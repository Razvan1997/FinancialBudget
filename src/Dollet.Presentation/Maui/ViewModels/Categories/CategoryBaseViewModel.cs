using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Dollet.Helpers;

namespace Dollet.ViewModels.Categories
{
    public abstract partial class CategoryBaseViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly ICategoryRepository _categoryRepository = unitOfWork.CategoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        private bool _isEnabled;

        public ObservableRangeCollection<Category> Categories { get; } = [];

        Category draggingCategory;

        [RelayCommand]
        protected virtual async Task Appearing()
        {
            var categories = await _categoryRepository.GetAllAsync();
            Categories.ReplaceRange(categories);
        }

        [RelayCommand]
        protected virtual void Disappearing()
        {
            Categories.Clear();
        }

        [RelayCommand]
        protected virtual void DragStarting(Category category)
        {
            draggingCategory = category;
        }

        [RelayCommand]
        protected virtual void DragOver(Category category)
        {
            var categoryToMove = draggingCategory;
            var categoryToInsertBefore = category;

            if (categoryToMove == null || categoryToInsertBefore == null || categoryToMove == categoryToInsertBefore)
                return;

            int insertAtIndex = Categories.IndexOf(categoryToInsertBefore);

            if (insertAtIndex >= 0 && insertAtIndex < Categories.Count)
            {
                Categories.Remove(categoryToMove);
                Categories.Insert(insertAtIndex, categoryToMove);
            }
        }

        [RelayCommand]
        protected virtual void Drop()
        {
            foreach (var tuple in Categories.Select((category, index) => (category, index).ToTuple()))
            {
                tuple.Item1.IndexOrder = tuple.Item2;
            }

            ChangeSaveEnabled();
        }

        [RelayCommand]
        protected virtual async Task Save()
        {
            try
            {
                _categoryRepository.UpdateMany(Categories);

                if (await _unitOfWork.CommitAsync())
                {
                    await Toast
                        .Make("Saved", ToastDuration.Long)
                        .Show();

                    ChangeSaveEnabled();
                }
            }
            catch
            {
                await Toast
                    .Make("Something went wrong...")
                    .Show();
            }
        }

        protected virtual void ChangeSaveEnabled() => IsEnabled = !IsEnabled;
    }
}
