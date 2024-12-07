using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;

namespace Dollet.ViewModels
{
    public partial class AppShellViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IAccountRepository _accountRepository = unitOfWork.AccountRepository;
        private readonly ICurrencyRepository _currencyRepository = unitOfWork.CurrencyRepository;

        [ObservableProperty]
        private decimal balance = 0;

        [ObservableProperty]
        private string currency = "PLN";

        [ObservableProperty]
        private bool _isLogoutVisible = true;

        [ObservableProperty]
        private bool _isCategoriesVisible = true;

        [RelayCommand]
        async Task Navigated()
        {
            Balance = 0;

            var defaultCurrency = await _currencyRepository.GetDefaultAsync();

            if (defaultCurrency is null)
                return;

            Currency = defaultCurrency.Code;

            var accounts = await _accountRepository.GetAllAsync();
            var currencyValues = await _currencyRepository.GetCurrencyValuesAsync(defaultCurrency.Code);

            foreach (var account in accounts.Where(x => !x.IsHidden))
            {
                currencyValues.TryGetValue(account.Currency, out decimal value);

                if (value == 0)
                {
                    continue;
                }

                var balance = account.Amount / value;
                Balance += balance;
            }
        }

        [RelayCommand]
        public async Task Logout()
        {
            unitOfWork.SetApplicationContext(null);
            await Shell.Current.GoToAsync("LoginPage");
        }

        [RelayCommand]
        static void ChangeAppTheme()
        {
            if (Application.Current is not null)
            {
                Application.Current.UserAppTheme = Application.Current.RequestedTheme is AppTheme.Dark
                                                    ? AppTheme.Light
                                                    : AppTheme.Dark;
            }
        }
    }
}