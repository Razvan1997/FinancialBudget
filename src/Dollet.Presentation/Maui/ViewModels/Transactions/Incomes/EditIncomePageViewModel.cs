using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Dollet.Core.Enums;
using Dollet.Helpers;

namespace Dollet.ViewModels.Transactions.Incomes
{
    public partial class EditIncomePageViewModel(IUnitOfWork unitOfWork) : ObservableObject, IQueryAttributable
    {
        private readonly IAccountRepository _accountRepository = unitOfWork.AccountRepository;
        private readonly ICategoryRepository _categoryRepository = unitOfWork.CategoryRepository;
        private readonly IIncomesRepository _incomesRepository = unitOfWork.IncomesRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private decimal initialAmount;
        private Account initialAccount;

        private int id;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private Account _selectedAccount;

        [ObservableProperty]
        private Category _selectedCategory;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private string _comment;
        
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
            var accounts = await _accountRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync(CategoryType.Income);

            Accounts.ReplaceRange(accounts);
            Categories.ReplaceRange(categories);

            var accountIndex = Accounts.IndexOf(Accounts.FirstOrDefault(x => x.Id == initialAccount.Id));
            var categoryIndex = Categories.IndexOf(Categories.FirstOrDefault(x => x.Id == SelectedCategory.Id));

            if (accountIndex > -1)
                SelectedAccount = Accounts[accountIndex];

            if (categoryIndex > -1)
                SelectedCategory = Categories[categoryIndex];
        }

        [RelayCommand]
        async Task EditIncome()
        {
            try
            {
                var selectedAccount = Accounts.FirstOrDefault(x => x.Id == SelectedAccount.Id);

                var difference = Amount - initialAmount;

                if (difference > 0)
                    selectedAccount.Amount += difference;
                
                else if (difference < 0)
                    selectedAccount.Amount -= Math.Abs(difference);

                var income = await _incomesRepository.GetAsync(id);

                income.Amount = Amount;
                income.Account = selectedAccount;
                income.Category = SelectedCategory;
                income.Date = Date;
                income.Comment = Comment;

                _incomesRepository.Update(income);
                _unitOfWork.AccountRepository.Update(selectedAccount);
                await _unitOfWork.CommitAsync();

                await Toast
                    .Make("Updated", ToastDuration.Long)
                    .Show();

                var navigationParameter = new ShellNavigationQueryParameters
                {
                    { "DateFrom", QueryParameterHelper.IncomesDetails.Item1 },
                    { "DateTo", QueryParameterHelper.IncomesDetails.Item2 },
                    { "CategoryId", QueryParameterHelper.IncomesDetails.Item3 }
                };

                await Shell.Current.GoToAsync("..", true, navigationParameter);
            }
            catch
            {
                await Toast
                    .Make("Something went wrong...", ToastDuration.Long)
                    .Show();
            }
        }
    }
}
