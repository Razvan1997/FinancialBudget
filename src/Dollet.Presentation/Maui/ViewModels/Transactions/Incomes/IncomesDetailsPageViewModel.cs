using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.DTOs;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.Pages.Transactions.Incomes;

namespace Dollet.ViewModels.Transactions.Incomes
{
    public partial class IncomesDetailsPageViewModel(IUnitOfWork unitOfWork) : ObservableObject, IQueryAttributable
    {
        private readonly IIncomesRepository _incomesRepository = unitOfWork.IncomesRepository;

        private int _categoryId;

        [ObservableProperty]
        private DateTime _dateFrom, _dateTo;

        public ObservableRangeCollection<IncomesDetailsGroupDto> Incomes { get; private set; } = [];

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            DateFrom = (DateTime)query["DateFrom"];
            DateTo = (DateTime)query["DateTo"];
            _categoryId = (int)query["CategoryId"];
        }

        [RelayCommand]
        async Task Appearing()
        {
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = false;

            var context = unitOfWork.GetApplicationContext();
            var accounts = await unitOfWork.AccountRepository.GetAsyncByUserAndPass(context.Name, context.Password);

            if (context.Role == Core.Enums.UserType.Admin)
            {
                var incomes = await _incomesRepository.GetAllAsync(DateFrom, DateTo, _categoryId, null);

                var incomesByDate = incomes
                    .GroupBy(p => p.Date)
                    .OrderByDescending(p => p.Key);

                foreach (var groupedModel in incomesByDate)
                {
                    Incomes.Replace(new IncomesDetailsGroupDto(groupedModel.Key, [.. groupedModel]));
                }
            }
            else
            {
                var incomes = await _incomesRepository.GetAllAsync(DateFrom, DateTo, _categoryId, accounts.FirstOrDefault().Id);

                var incomesByDate = incomes
                    .GroupBy(p => p.Date)
                    .OrderByDescending(p => p.Key);

                foreach (var groupedModel in incomesByDate)
                {
                    Incomes.Replace(new IncomesDetailsGroupDto(groupedModel.Key, [.. groupedModel]));
                }
            }
        }

        [RelayCommand]
        async Task GoToEditIncome(Income income)
        {
            await Shell.Current.GoToAsync($"{nameof(EditIncomePage)}", new ShellNavigationQueryParameters {
                { "Id", income.Id},
                { "Amount", income.Amount},
                { "Account", income.Account},
                { "SelectedCategory", income.Category},
                { "Date", income.Date},
                { "Comment", income.Comment},
            });
        }
    }
}
