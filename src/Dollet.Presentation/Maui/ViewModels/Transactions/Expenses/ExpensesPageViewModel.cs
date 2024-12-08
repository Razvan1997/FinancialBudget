using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DTOs;
using Dollet.Core.Entities;
using Dollet.Core.Helpers;
using Dollet.Helpers;
using Dollet.Pages.Transactions.Expenses;
using Microcharts;
using SkiaSharp;

namespace Dollet.ViewModels.Transactions.Expenses
{
    public partial class ExpensesPageViewModel(IUnitOfWork unitOfWork, IPopupService popupService, PeriodsHelper periodsHelper) : TransactionBaseViewModel(popupService, periodsHelper)
    {
        private readonly IExpensesRepository _expensesRepository = unitOfWork.ExpensesRepository;
        private readonly IPopupService _popupService = popupService;
        public Currency _defaultCurrency;
        public ObservableRangeCollection<ExpensesGroupDto> Expenses { get; } = [];
        [ObservableProperty]
        private bool _isVisiblePeriods;
        [RelayCommand]
        async Task Appearing()
        {
            var context = unitOfWork.GetApplicationContext();

            if (context.Role == Core.Enums.UserType.Normal)
            {
                IsVisiblePeriods = false;
            }
            else
            {
                IsVisiblePeriods = true;
            }

            _defaultCurrency = await unitOfWork.CurrencyRepository.GetDefaultAsync();

            DonutChart = GetDonutChart();

            CalculateDateRange();

            await CalculateExpensesAsync();
        }

        [RelayCommand]
        async Task PeriodChanged()
        {
            CalculateDateRange();

            await CalculateExpensesAsync();
        }

        public override async Task SelectCustomDateRange()
        {
            await base.SelectCustomDateRange();

            await CalculateExpensesAsync();
        }

        private async Task CalculateExpensesAsync()
        {
            await ClearChartData();

            var groupedExpenses = new List<ExpensesGroupDto>();
            var entries = new List<ChartEntry>();

            var expenses = await _expensesRepository.GetAllAsync(_dateFrom, _dateTo);

            if (expenses.Any())
            {
                var expensesSum = expenses.Sum(p => p.Amount);

                foreach (var groupedModel in expenses.GroupBy(p => p.CategoryId))
                {
                    var groupedSum = groupedModel.Sum(p => p.Amount);
                    var category = groupedModel.FirstOrDefault()?.Category;

                    if (expensesSum != 0)
                    {
                        var percentValue = groupedSum / expensesSum * 100;

                        groupedExpenses.Add(new ExpensesGroupDto
                        {
                            Category = category?.Name,
                            CategoryId = category?.Id,
                            Amount = groupedSum,
                            Percent = percentValue,
                            Icon = category.Icon,
                            Color = category.Color,
                            DefaultCurrency = _defaultCurrency.Code
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

                Expenses.AddRange(groupedExpenses.OrderByDescending(x => x.Amount));
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

        [RelayCommand]
        async Task NavigateToExpensesDetailsPage(ExpensesGroupDto expense)
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "DateFrom", _dateRange.Item1 },
                { "DateTo", _dateRange.Item2 },
                { "CategoryId", expense.CategoryId }
            };

            QueryParameterHelper.ExpensesDetails = (_dateRange.Item1, _dateRange.Item2, expense.CategoryId.Value);

            await Shell.Current.GoToAsync(nameof(ExpensesDetailsPage), true, navigationParameter);
        }

        [RelayCommand]
        async Task NavigateToAddExpensePage()
        {
            await Shell.Current.GoToAsync(nameof(AddExpensePage));
        }

        public override async Task ClearChartData()
        {
            DonutChart = null;
            Expenses.Clear();
            await Task.Delay(100);
        }
    }
}