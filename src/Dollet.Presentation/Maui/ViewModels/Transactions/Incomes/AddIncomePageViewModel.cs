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
    public partial class AddIncomePageViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IAccountRepository _accountRepository = unitOfWork.AccountRepository;
        private readonly ICategoryRepository _categoryRepository = unitOfWork.CategoryRepository;
        private readonly IIncomesRepository _incomesRepository = unitOfWork.IncomesRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        private Account _selectedAccount;

        public decimal Amount { get; set; }
        public Category SelectedCategory { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime MaximumDate { get; } = DateTime.Now;

        public ObservableRangeCollection<Account> Accounts { get; } = [];
        public ObservableRangeCollection<Category> Categories { get; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            var accounts = await _accountRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync(CategoryType.Income);

            Accounts.AddRange(accounts);
            Categories.AddRange(categories);

            SelectedAccount = accounts.FirstOrDefault(x => x.IsDefault);
        }

        [RelayCommand]
        async Task AddIncome()
        {
            try
            {
                var selectedAccount = Accounts.FirstOrDefault(x => x.Id == SelectedAccount.Id);

                selectedAccount.Amount += Amount;

                _accountRepository.Update(selectedAccount);
                _incomesRepository.Add(new Income
                {
                    Amount = Amount,
                    AccountId = SelectedAccount.Id,
                    CategoryId = SelectedCategory.Id,
                    Date = Date,
                    Comment = Comment
                });

                await _unitOfWork.CommitAsync();

                await Toast
                    .Make("Added", ToastDuration.Long)
                    .Show();

                await Shell.Current.GoToAsync("..");
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
