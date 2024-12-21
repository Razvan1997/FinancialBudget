using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DTOs;
using Dollet.Core.Helpers;
using Dollet.Helpers;
using Dollet.Pages.Transactions.Incomes;
using Microcharts;
using SkiaSharp;

namespace Dollet.ViewModels.Transactions.Incomes
{
    public partial class IncomesPageViewModel(IUnitOfWork unitOfWork, IPopupService popupService, PeriodsHelper periodsHelper) : TransactionBaseViewModel(popupService, periodsHelper)
    {
        private readonly IIncomesRepository _incomesRepository = unitOfWork.IncomesRepository;
        private readonly IPopupService _popupService = popupService;
        [ObservableProperty]
        private bool _isVisiblePeriods;
        public ObservableRangeCollection<IncomesGroupDto> Incomes { get; } = [];
        [ObservableProperty]
        private bool _isAvailable;
        [RelayCommand]
        async Task Appearing()
        {
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = false;
            var context = unitOfWork.GetApplicationContext();

            if (context.Role == Core.Enums.UserType.Normal)
            {
                IsVisiblePeriods = false;
                IsAvailable = true;
            }
            else
            {
                IsVisiblePeriods = true;
                IsAvailable = false;
            }

            DonutChart = GetDonutChart();

            CalculateDateRange();

            await CalculateIncomesAsync();
        }

        [RelayCommand]
        async Task PeriodChanged()
        {
            CalculateDateRange();

            await CalculateIncomesAsync();
        }

        public override async Task SelectCustomDateRange()
        {
            await base.SelectCustomDateRange();

            await CalculateIncomesAsync();
        }

        private async Task CalculateIncomesAsync()
        {
            await ClearChartData();

            var groupedIncomes = new List<IncomesGroupDto>();
            var entries = new List<ChartEntry>();

            var context = unitOfWork.GetApplicationContext();

            if(context.Role == Core.Enums.UserType.Admin)
            {
                var incomes = await _incomesRepository.GetAllAsync(_dateFrom, _dateTo, null, null);

                if (incomes.Any())
                {
                    var incomesSum = incomes.Sum(p => p.Amount);

                    foreach (var groupedModel in incomes.GroupBy(p => p.CategoryId))
                    {
                        var groupedSum = groupedModel.Sum(p => p.Amount);
                        var category = groupedModel.FirstOrDefault()?.Category;

                        if (incomesSum != 0)
                        {
                            var percentValue = groupedSum / incomesSum * 100;

                            groupedIncomes.Add(new IncomesGroupDto
                            {
                                Category = category?.Name,
                                CategoryId = category?.Id,
                                Amount = groupedSum,
                                Percent = percentValue,
                                Icon = category.Icon,
                                Color = category.Color
                            });

                            entries.Add(new ChartEntry((float)percentValue)
                            {
                                Label = category?.Name,
                                ValueLabel = groupedSum.ToString("0.00"),
                                Color = SKColor.Parse(category.Color),
                                ValueLabelColor = SKColor.Parse(category.Color)
                            });
                        }
                    }

                    Incomes.AddRange(groupedIncomes.OrderByDescending(x => x.Amount));
                    DonutChart = GetDonutChart(entries);
                }
                else
                {
                    entries.Add(new ChartEntry(100)
                    {
                        Label = "No data",
                        ValueLabel = "0",
                        Color = SKColor.Parse("#ACACAC")
                    });

                    DonutChart = GetDonutChart(entries);
                }
            }
            else
            {
                var accounts = await unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);
                var incomes = await _incomesRepository.GetAllAsync(_dateFrom, _dateTo, null, accounts.FirstOrDefault().Id);

                if (incomes.Any())
                {
                    var incomesSum = incomes.Sum(p => p.Amount);

                    foreach (var groupedModel in incomes.GroupBy(p => p.CategoryId))
                    {
                        var groupedSum = groupedModel.Sum(p => p.Amount);
                        var category = groupedModel.FirstOrDefault()?.Category;

                        if (incomesSum != 0)
                        {
                            var percentValue = groupedSum / incomesSum * 100;

                            groupedIncomes.Add(new IncomesGroupDto
                            {
                                Category = category?.Name,
                                CategoryId = category?.Id,
                                Amount = groupedSum,
                                Percent = percentValue,
                                Icon = category.Icon,
                                Color = category.Color
                            });

                            entries.Add(new ChartEntry((float)percentValue)
                            {
                                Label = category?.Name,
                                ValueLabel = groupedSum.ToString("0.00"),
                                Color = SKColor.Parse(category.Color),
                                ValueLabelColor = SKColor.Parse(category.Color)
                            });
                        }
                    }

                    Incomes.AddRange(groupedIncomes.OrderByDescending(x => x.Amount));
                    DonutChart = GetDonutChart(entries);
                }
                else
                {
                    entries.Add(new ChartEntry(100)
                    {
                        Label = "No data",
                        ValueLabel = "0",
                        Color = SKColor.Parse("#ACACAC")
                    });

                    DonutChart = GetDonutChart(entries);
                }
            }
        }

        [RelayCommand]
        async Task NavigateToIncomesDetailsPage(IncomesGroupDto income)
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "DateFrom", _dateRange.Item1 },
                { "DateTo", _dateRange.Item2 },
                { "CategoryId", income.CategoryId }
            };

            QueryParameterHelper.IncomesDetails = (_dateRange.Item1, _dateRange.Item2, income.CategoryId.Value);

            await Shell.Current.GoToAsync(nameof(IncomesDetailsPage), true, navigationParameter);
        }

        [RelayCommand]
        async Task NavigateToAddIncomePage()
        {
            await Shell.Current.GoToAsync(nameof(AddIncomePage));
        }

        public override async Task ClearChartData()
        {
            DonutChart = null;
            Incomes.Clear();
            await Task.Delay(100);
        }
    }
}
