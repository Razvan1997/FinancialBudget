using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Services;
using Dollet.Core.Entities;
using Dollet.Pages;

namespace Dollet.ViewModels
{
    internal partial class LoadingPageViewModel(IDataSeedService dataSeedService, IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IDataSeedService _dataSeedService = dataSeedService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        bool _isBusy = true;

        [RelayCommand]
        async Task Appearing()
        {
            try
            {
                await _dataSeedService.SeedCategoriesAsync();
                await _dataSeedService.SeedCurrenciesAsync();
                await _dataSeedService.SeedUsersAsync();

                if (await CanSeed())
                {
                    await _dataSeedService.SeedCurrencyValuesAsync();
                }

                await UpdateLastRun();
            }
            catch (Exception)
            {
                throw;
            }

            ChangeBusyState();

            await Shell.Current.GoToAsync("LoginPage");
        }

        void ChangeBusyState() => IsBusy = !IsBusy;

        async Task UpdateLastRun()
        {
            await _unitOfWork.AppDataRepository.UpdateAsync(new AppData { LastRun = DateTime.Now.ToLocalTime() });
            await _unitOfWork.CommitAsync();
        }

        async Task<bool> CanSeed()
        {
            var lastRun = (await _unitOfWork.AppDataRepository.GetAsync())?.LastRun;

            if (lastRun is null)
            {
                return true;
            }

            var today = DateTime.Now.Date;

            return lastRun <= today.AddDays(-1);
        }
    }
}