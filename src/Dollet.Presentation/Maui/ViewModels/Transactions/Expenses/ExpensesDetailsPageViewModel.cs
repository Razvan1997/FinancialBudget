using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DTOs;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.Pages.Transactions.Expenses;
using Dollet.ViewModels.Dtos;

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
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = false;
            Expenses.Clear();

            var context = unitOfWork.GetApplicationContext();
            var accounts = await unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);

            if (context.Role == Core.Enums.UserType.Admin)
            {
                var expenses = await _expensesRepository.GetAllAsync(DateFrom, DateTo, _categoryId, null);
                var expensesByDate = expenses
                    .GroupBy(p => p.Date)
                    .OrderByDescending(p => p.Key);

                foreach (var groupedModel in expensesByDate)
                {
                    Expenses.Add(new ExpensesDetailsGroupDto(groupedModel.Key, [.. groupedModel]));
                }
            }
            else
            {
                var expenses = await _expensesRepository.GetAllAsync(DateFrom, DateTo, _categoryId, accounts.FirstOrDefault().Id);
                var expensesByDate = expenses
                    .GroupBy(p => p.Date)
                    .OrderByDescending(p => p.Key);

                foreach (var groupedModel in expensesByDate)
                {
                    Expenses.Add(new ExpensesDetailsGroupDto(groupedModel.Key, [.. groupedModel]));
                }
            }
        }

        [RelayCommand]
        async Task GoToEditExpense(Expense expense)
        {
            var context = unitOfWork.GetApplicationContext();

            var accounts = await unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);

            var category = await unitOfWork.AccountCategoryRepository.GetCategoryByAccountIdAndCategoryIdAsync(accounts.FirstOrDefault().Id, expense.CategoryId);

            var categoryDto = new CategoryDto()
            {
                Id = expense.CategoryId,
                Color = expense.Category.Color,
                Icon = expense.Category.Icon,
                Name = expense.Category.Name,
                IsSelected = false,
                Budget = category.Budget
            };

            await Shell.Current.GoToAsync($"{nameof(EditExpensePage)}", new ShellNavigationQueryParameters {
                { "Id", expense.Id},
                { "Amount", expense.Amount},
                { "Account", expense.Account},
                { "SelectedCategory", categoryDto},
                { "Date", expense.Date},
                { "Comment", expense.Comment},
            });
        }
    }
}
