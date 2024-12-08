using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.ViewModels.Popups
{
    public partial class CategorySelectedPopupViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICategoryRepository _categoryRepository = unitOfWork.CategoryRepository;

        [ObservableProperty]
        private string _selectedCategoryName;

        [ObservableProperty]
        private decimal _budgetValue;

        // Stocare temporară: ID categorie, buget și ID cont
        private readonly Dictionary<int, (decimal Budget, int AccountId)> _categoriesCache = new();

        private Category Category;
        public Popup Popup { get; set; }

        [RelayCommand]
        private void Confirm()
        {
            if (Category != null)
            {
                if (!_categoriesCache.ContainsKey(Category.Id))
                {
                    _categoriesCache.Add(Category.Id, (BudgetValue, 0));
                }
                else
                {
                    var existingData = _categoriesCache[Category.Id];
                    _categoriesCache[Category.Id] = (BudgetValue, existingData.AccountId);
                }

                ClosePopup();
            }
        }

        [RelayCommand]
        private void Dismiss()
        {
            if (_categoriesCache.ContainsKey(Category.Id))
            {
                _categoriesCache.Remove(Category.Id);
            }
            ClosePopup();
        }

        public async void SetSelectedCategory(int categoryId, bool editMode, int accountId = 0)
        {
            if (_categoriesCache.ContainsKey(categoryId))
            {
                var cachedData = _categoriesCache[categoryId];
                BudgetValue = cachedData.Budget;
                SelectedCategoryName = _categoryRepository.GetCategoryByIdAsync(categoryId).Result.Name;
            }
            else
            {
                if (editMode)
                {
                    var allCategoriesExpenses = await _unitOfWork.CategoryRepository.GetAllAsync();
                    var categories = await _unitOfWork.AccountCategoryRepository.GetCategoriesByAccountIdAsync(accountId);
                    var currentCategory = categories.FirstOrDefault(x => x.CategoryId == categoryId);
                    if (currentCategory != null)
                    {
                        Category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                        if (Category != null)
                        {
                            SelectedCategoryName = Category.Name;
                            BudgetValue = currentCategory.Budget;
                        }
                    }
                    else
                    {
                        Category = allCategoriesExpenses.FirstOrDefault(x => x.Id == categoryId);
                        if (Category != null)
                        {
                            SelectedCategoryName = Category.Name;
                            BudgetValue = 0;
                        }
                    }
                }
                else
                {
                    Category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                    if (Category != null)
                    {
                        SelectedCategoryName = Category.Name;
                        BudgetValue = 0;
                    }
                }
            }
        }

        public void UndoSelectedCategory(int categoryId)
        {
            if (_categoriesCache.ContainsKey(categoryId))
            {
                _categoriesCache.Remove(categoryId);
            }
        }

        public void UpdateAccountIdForAllCategories(int accountId)
        {
            foreach (var categoryId in _categoriesCache.Keys.ToList())
            {
                var currentData = _categoriesCache[categoryId];
                _categoriesCache[categoryId] = (currentData.Budget, accountId);
            }
        }

        private void ClosePopup()
        {
            Popup.CloseAsync();
        }

        public void Clear()
        {
            _categoriesCache.Clear();
        }

        public Dictionary<int, (decimal Budget, int AccountId)> GetCacheData()
        {
            return _categoriesCache;
        }
    }
}
