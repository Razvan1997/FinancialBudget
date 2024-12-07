using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Models;

namespace Dollet.ViewModels.Popups
{
    public partial class DateRangePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime
            _dateFrom = DateTime.Today, _dateTo = DateTime.Today,
            _maxDateFrom = DateTime.Today, _maxDateTo = DateTime.Today;

        [RelayCommand]
        async Task Confirm(Popup popup) => await popup.CloseAsync(new ConfirmDateRangeResult(DateFrom, DateTo));

        [RelayCommand]
        static async Task Dismiss(Popup popup) => await popup.CloseAsync(null);
    }
}