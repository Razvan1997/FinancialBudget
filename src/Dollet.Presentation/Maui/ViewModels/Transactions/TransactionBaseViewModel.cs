using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Enums;
using Dollet.Core.Helpers;
using Dollet.Helpers;
using Dollet.Models;
using Dollet.ViewModels.Popups;
using Microcharts;
using SkiaSharp;

namespace Dollet.ViewModels.Transactions
{
    public abstract partial class TransactionBaseViewModel(IPopupService popupService, PeriodsHelper periodsHelper) : ObservableObject
    {
        private readonly IPopupService _popupService = popupService;
        private readonly PeriodsHelper _periodsHelper = periodsHelper;
        
        public ObservableRangeCollection<string> Periods { get; }
            = new ObservableRangeCollection<string>(
                Enum.GetValues<TransactionsPeriod>().Select(x => x.ToString()));

        [ObservableProperty]
        public DonutChart _donutChart;
        
        public DateTime _dateFrom = DateTime.Today, _dateTo = DateTime.Today;
        public (DateTime, DateTime) _dateRange;

        public string SelectedPeriod { get; set; } = TransactionsPeriod.Daily.ToString();

        [ObservableProperty]
        private string _period;

        [RelayCommand]
        public virtual async Task SelectCustomDateRange()
        {
            var result = await _popupService.ShowPopupAsync<DateRangePopupViewModel>();

            if (result is null)
                return;

            var dateRange = (ConfirmDateRangeResult)result;

            _dateRange = (dateRange.DateFrom, dateRange.DateTo);

            CalculateDateRange(isCustom: true);
        }

        public virtual DonutChart GetDonutChart(List<ChartEntry> entries = null)
        {
            var chart = new DonutChart()
            {
                GraphPosition = GraphPosition.Center,
                Typeface = SKTypeface.FromFamilyName("VactorySansRegular"),
                BackgroundColor = SKColor.Empty,
                AnimationDuration = TimeSpan.FromMilliseconds(250),
                IsAnimated = true,
                LabelTextSize = 28
            };

            if (entries is not null)
            {
                chart.Entries = entries;
            }

            return chart;
        }

        public void CalculateDateRange(bool isCustom = false)
        {
            if (!isCustom)
                _dateRange = _periodsHelper.GetDateRangeBasedOnSelectedPeriod(SelectedPeriod);

            _dateFrom = _dateRange.Item1;
            _dateTo = _dateRange.Item2;

            Period = PeriodsHelper.GetStringPeriod(_dateRange);
        }

        public abstract Task ClearChartData();
    }
}