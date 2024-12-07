using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Models;

namespace Dollet.ViewModels.Popups
{
    public partial class ConfirmPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand), nameof(ConfirmForceCommand))]
        double _progress = 0.00d;

        public ConfirmPopupViewModel()
        {
            Dispatcher.GetForCurrentThread().StartTimer(new TimeSpan(0, 0, 0, 0, 10), () =>
            {
                if(Progress >= 1)
                    return false;

                Progress += 0.004;
                return true;
            });
        }

        [RelayCommand(CanExecute = nameof(ProgressBarElapsed))]
        static async Task ConfirmForce(Popup popup) => await popup.CloseAsync(new ConfirmResult(true, true));

        [RelayCommand(CanExecute = nameof(ProgressBarElapsed))]
        static async Task Confirm(Popup popup) => await popup.CloseAsync(new ConfirmResult(false, true));

        [RelayCommand]
        static async Task Dismiss(Popup popup) => await popup.CloseAsync(new ConfirmResult(false, false));
        
        bool ProgressBarElapsed() => Progress >= 1;
    }
}
