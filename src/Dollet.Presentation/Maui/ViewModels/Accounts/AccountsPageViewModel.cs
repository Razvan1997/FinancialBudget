using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Constants;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.Pages;
using Dollet.Pages.Accounts;
using Dollet.ViewModels.Dtos;

namespace Dollet.ViewModels.Accounts
{
    public partial class AccountsPageViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private decimal accountsBalance = 0.00m;
        private bool hiddenAccountsExpanded = false;
        private string selectedCurrency = "PLN", hiddenAccountsIcon = MaterialDesignIcons.Arrow_left;
        private Currency _defaultCurrency;
        private Dictionary<string, decimal> _currencyValues;

        public decimal AccountsBalance
        {
            get => accountsBalance;
            set => SetProperty(ref accountsBalance, value);
        }
        public string SelectedCurrency
        {
            get => selectedCurrency;
            set => SetProperty(ref selectedCurrency, value);
        }
        public bool HiddenAccountsExpanded
        {
            get => hiddenAccountsExpanded;
            set
            {
                if (value)
                    HiddenAccountsIcon = MaterialDesignIcons.Arrow_drop_down;
                else
                    HiddenAccountsIcon = MaterialDesignIcons.Arrow_left;

                SetProperty(ref hiddenAccountsExpanded, value);
            }
        }

        public string HiddenAccountsIcon
        {
            get { return hiddenAccountsIcon; }
            set { SetProperty(ref hiddenAccountsIcon, value); }
        }

        private bool _isAddAccountButtonEnabled = true;
        public bool IsAddAccountButtonEnabled
        {
            get => _isAddAccountButtonEnabled;
            set => SetProperty(ref _isAddAccountButtonEnabled, value);
        }

        public ObservableRangeCollection<AccountDto> Accounts { get; } = [];
        public ObservableRangeCollection<AccountDto> HiddenAccounts { get; } = [];
        public ObservableRangeCollection<string> Currencies { get; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = true;

            Accounts.Clear();

            _defaultCurrency = await _unitOfWork.CurrencyRepository.GetDefaultAsync();
            _currencyValues = await _unitOfWork.CurrencyRepository.GetCurrencyValuesAsync(_defaultCurrency.Code);

            var context = _unitOfWork.GetApplicationContext();
            
            IEnumerable<Account> accounts = null;
            if (context.Role == Core.Enums.UserType.Admin)
            {
                accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            }
            else
            {
                accounts = await _unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);
            }
            
            foreach (var item in accounts.GroupBy(r => r.IsHidden))
            {
                if (item.Key)
                {
                    HiddenAccounts.ReplaceRange(MapToDtos(item));
                    continue;
                }
                Accounts.ReplaceRange(MapToDtos(item));
            }

            var currencies = _currencyValues.Select(f => f.Key);
            Currencies.ReplaceRange(currencies);

            if (context.Role == Core.Enums.UserType.Normal)
            {
                IsAddAccountButtonEnabled = false;
            }
            else
            {
                IsAddAccountButtonEnabled = true;
            }

            CurrencyChanged();
        }

        private static IEnumerable<AccountDto> MapToDtos(IEnumerable<Account> accounts)
        {
            return accounts
                .Select(x => new AccountDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Name = x.Name,
                    Description = x.Description,
                    Icon = x.Icon,
                    Color = x.Color,
                    Currency = x.Currency,
                    IsHidden = x.IsHidden,
                    IsDefault = x.IsDefault,
                    Username = x.Username,
                    Password = x.Password,
                })
                .ToList();
        }

        [RelayCommand]
        void CurrencyChanged()
        {
            AccountsBalance = 0;

            foreach (var account in Accounts.Where(x => !x.IsHidden))
            {
                //_currencyValues.TryGetValue(account.Currency, out decimal value);

                //if (value == 0)
                //    continue;

                var balance = account.Amount;
                AccountsBalance += balance;
            }
            SelectedCurrency = _defaultCurrency.Code;
        }

        [RelayCommand]
        void ToogleHiddenAccounts() => HiddenAccountsExpanded = !HiddenAccountsExpanded;

        [RelayCommand]
        async Task NavigateToAddAccountPage()
        {
            await Shell.Current.GoToAsync(nameof(AddAccountPage));
        }

        [RelayCommand]
        async Task NavigateToEditAccountPage(AccountDto account)
        {
            if (IsAddAccountButtonEnabled)
            {
                await Shell.Current.GoToAsync(nameof(EditAccountPage), true, new Dictionary<string, object>
                {
                    {"Account", account }
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Nu poti edita datele contului. Vorbeste cu Administratorul contului!", "OK");
            }
        }
    }
}
