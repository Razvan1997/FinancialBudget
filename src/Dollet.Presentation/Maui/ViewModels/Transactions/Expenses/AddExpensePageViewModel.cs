using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.ViewModels.Dtos;

namespace Dollet.ViewModels.Transactions.Expenses
{
    public partial class AddExpensePageViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        private Account _selectedAccount;
        public decimal Amount { get; set; }
        public CategoryDto SelectedCategory { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime MaximumDate { get; } = DateTime.Now;

        public ObservableRangeCollection<Account> Accounts { get; } = [];
        public ObservableRangeCollection<CategoryDto> Categories { get; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            var context = _unitOfWork.GetApplicationContext();

            var accounts = await _unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password) ;

            var categories = await _unitOfWork.AccountCategoryRepository.GetCategoriesByAccountIdAsync(accounts.FirstOrDefault().Id);

            Accounts.ReplaceRange(accounts);
            foreach (var category in categories)
            {
                var item = _unitOfWork.CategoryRepository.GetCategoryByIdAsync(category.CategoryId).Result;
                var categoryDto = new CategoryDto()
                {
                    Id = item.Id,
                    Color = item.Color,
                    Icon = item.Icon,
                    Name = item.Name,
                    IsSelected = false,
                    Budget = category.Budget
                };
                Categories.Add(categoryDto);
            }

            SelectedAccount = accounts.FirstOrDefault(x => x.IsDefault);
        }

        [RelayCommand]
        async Task AddExpense()
        {
            var selectedAccount = Accounts.FirstOrDefault(x => x.Id == SelectedAccount.Id);
            var selectedCategory = Categories.FirstOrDefault( x=> x.Id == SelectedCategory.Id);

            if(Amount > selectedCategory.Budget)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Suma introdusa depaseste bugetul pe categoria selectata.", "OK");
                return;
            }
            else
            {
                selectedAccount.Amount -= Amount;
                _unitOfWork.AccountRepository.Update(selectedAccount);
                _unitOfWork.ExpensesRepository.Add(new Expense
                {
                    Amount = Amount,
                    AccountId = SelectedAccount.Id,
                    CategoryId = SelectedCategory.Id,
                    Date = Date,
                    Comment = Comment
                });

                try
                {
                    await _unitOfWork.CommitAsync();
                    await Toast
                   .Make("Added", ToastDuration.Long)
                   .Show();

                    await Shell.Current.GoToAsync("..");
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
