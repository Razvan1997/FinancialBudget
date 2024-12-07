using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Services;
using Dollet.Core.Entities;
using Dollet.Helpers;

namespace Dollet.ViewModels
{
    public partial class CurrenciesPageViewModel(IUnitOfWork unitOfWork, IDataSeedService dataSeedService) : ObservableObject
    {
        private readonly IDataSeedService _dataSeedService = dataSeedService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public ObservableRangeCollection<Currency> Currencies { get; } = [];

        bool isChangeCurrencyEnabled = false;
        public bool IsChangeCurrencyEnabled
        {
            get => isChangeCurrencyEnabled; 
            set => SetProperty(ref isChangeCurrencyEnabled, value);
        }

        Currency _defaultCurrency;
        Currency selectedCurrency;
        public Currency SelectedCurrency 
        { 
            get => selectedCurrency; 
            set => SetProperty(ref selectedCurrency, value);
        }

        [RelayCommand]
        async Task Appearing()
        {
            var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();
            var defaultCurrency = currencies.FirstOrDefault(x => x.IsDefault);

            _defaultCurrency = defaultCurrency;

            SelectedCurrency = defaultCurrency;
            Currencies.ReplaceRange(currencies);
        }

        [RelayCommand]
        void DefaultCurrencyChanged()
        {
            if(SelectedCurrency?.Code == _defaultCurrency.Code)
            {
                IsChangeCurrencyEnabled = false;
            }
            else
            {
                IsChangeCurrencyEnabled = true;
            }
        }

        [RelayCommand]
        async Task ChangeDefaultCurrency()
        {
            try
            {
                _defaultCurrency.IsDefault = false;
                SelectedCurrency.IsDefault = true;

                _unitOfWork.CurrencyRepository.Update(_defaultCurrency);
                _unitOfWork.CurrencyRepository.Update(SelectedCurrency);

                await _dataSeedService.SeedCurrencyValuesAsync(SelectedCurrency.Code);

                await Toast
                    .Make("Default currency changed", ToastDuration.Long)
                    .Show();

                _defaultCurrency = SelectedCurrency;
                IsChangeCurrencyEnabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
