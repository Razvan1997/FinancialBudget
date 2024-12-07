using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Entities;
using Dollet.Helpers;

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
        Category _selectedCategory;

        [ObservableProperty]
        DateTime _date;

        [ObservableProperty]
        string _comment;

        public DateTime MaximumDate { get; } = DateTime.Now;

        public ObservableRangeCollection<Account> Accounts { get; } = [];
        public ObservableRangeCollection<Category> Categories { get; } = [];

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            id = (int)query["Id"];
            Amount = (decimal)query["Amount"];
            SelectedCategory = (Category)query["SelectedCategory"];
            Date = (DateTime)query["Date"];
            Comment = (string)query["Comment"];

            initialAccount = (Account)query["Account"];
            initialAmount = (decimal)query["Amount"];
        }

        [RelayCommand]
        async Task Appearing()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            Accounts.ReplaceRange(accounts);
            Categories.ReplaceRange(categories);

            var accountIndex = Accounts.IndexOf(Accounts.FirstOrDefault(x => x.Id == initialAccount.Id));
            var categoryIndex = Categories.IndexOf(Categories.FirstOrDefault(x => x.Id == SelectedCategory.Id));

            if(accountIndex > -1)
                SelectedAccount = Accounts[accountIndex];

            if (categoryIndex > -1)
                SelectedCategory = Categories[categoryIndex];
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

            expense.Amount = Amount.Value;
            expense.Account = selectedAccount;
            expense.Category = SelectedCategory;
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
