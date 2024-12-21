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
    public partial class EditExpensePageViewModel(IUnitOfWork unitOfWork) : ObservableObject, IQueryAttributable
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private decimal initialAmount;
        private Account initialAccount;

        private int id;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditExpenseCommand))]
        decimal? _amount;

        [ObservableProperty]
        Account _selectedAccount;
        
        [ObservableProperty]
        CategoryDto _selectedCategory;

        [ObservableProperty]
        DateTime _date;

        [ObservableProperty]
        string _comment;

        public DateTime MaximumDate { get; } = DateTime.Now;

        public ObservableRangeCollection<Account> Accounts { get; } = [];
        public ObservableRangeCollection<CategoryDto> Categories { get; } = [];

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            id = (int)query["Id"];
            Amount = (decimal)query["Amount"];
            SelectedCategory = (CategoryDto)query["SelectedCategory"];
            Date = (DateTime)query["Date"];
            Comment = (string)query["Comment"];

            initialAccount = (Account)query["Account"];
            initialAmount = (decimal)query["Amount"];
        }

        [RelayCommand]
        async Task Appearing()
        {
            var context = _unitOfWork.GetApplicationContext();

            if (context.Role == Core.Enums.UserType.Normal)
            {
                var accounts = await _unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);

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

                var accountIndex = Accounts.IndexOf(Accounts.FirstOrDefault(x => x.Id == initialAccount.Id));
                var categoryIndex = Categories.IndexOf(Categories.FirstOrDefault(x => x.Id == SelectedCategory.Id));

                if (accountIndex > -1)
                    SelectedAccount = Accounts[accountIndex];

                if (categoryIndex > -1)
                    SelectedCategory = Categories[categoryIndex];
            }
        }

        [RelayCommand(CanExecute = nameof(CanEditExpense))]
        async Task EditExpense()
        {
            var selectedAccount = Accounts.FirstOrDefault(x => x.Id == SelectedAccount.Id);

            var difference = (Amount ?? 0) - initialAmount;

            if (difference > 0)
                selectedAccount.Amount -= difference;            
            else if(difference < 0)
                selectedAccount.Amount += Math.Abs(difference);
            
            var expense = await _unitOfWork.ExpensesRepository.GetAsync(id);

            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(SelectedCategory.Id);

            expense.Amount = Amount.Value;
            expense.Account = selectedAccount;
            expense.Category = category;
            expense.Date = Date;
            expense.Comment = Comment;

            _unitOfWork.ExpensesRepository.Update(expense);
            _unitOfWork.AccountRepository.Update(selectedAccount);
            await _unitOfWork.CommitAsync();

            await Toast
                .Make("Updated", ToastDuration.Long)
                .Show();

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "DateFrom", QueryParameterHelper.ExpensesDetails.Item1 },
                { "DateTo", QueryParameterHelper.ExpensesDetails.Item2 },
                { "CategoryId", QueryParameterHelper.ExpensesDetails.Item3 }
            };

            await Shell.Current.GoToAsync("..", true, navigationParameter);
        }

        bool CanEditExpense() => Amount.HasValue;
    }
}
