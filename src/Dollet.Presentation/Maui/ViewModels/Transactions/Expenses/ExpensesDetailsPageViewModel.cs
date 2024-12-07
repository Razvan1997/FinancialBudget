using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DTOs;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.Pages.Transactions.Expenses;

namespace Dollet.ViewModels.Transactions.Expenses
{
    public partial class ExpensesDetailsPageViewModel(IUnitOfWork unitOfWork) : ObservableObject, IQueryAttributable
    {
        private readonly IExpensesRepository _expensesRepository = unitOfWork.ExpensesRepository;
        
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            DateFrom = (DateTime)query["DateFrom"];
            DateTo = (DateTime)query["DateTo"];
            _categoryId = (int)query["CategoryId"];
        }

        private int _categoryId;

        [ObservableProperty]
        private DateTime _dateFrom, _dateTo;

        public ObservableRangeCollection<ExpensesDetailsGroupDto> Expenses { get; private set; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            Expenses.Clear();

            var expenses = await _expensesRepository.GetAllAsync(DateFrom, DateTo, _categoryId);
            var expensesByDate = expenses
                .GroupBy(p => p.Date)
                .OrderByDescending(p => p.Key);

            foreach (var groupedModel in expensesByDate)
            {
                Expenses.Add(new ExpensesDetailsGroupDto(groupedModel.Key, [.. groupedModel]));
            }
        }

        [RelayCommand]
        async Task GoToEditExpense(Expense expense)
        {
            await Shell.Current.GoToAsync($"{nameof(EditExpensePage)}", new ShellNavigationQueryParameters {
                { "Id", expense.Id},
                { "Amount", expense.Amount},
                { "Account", expense.Account},
                { "SelectedCategory", expense.Category},
                { "Date", expense.Date},
                { "Comment", expense.Comment},
            });
        }
    }
}
