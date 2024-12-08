using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.DomainServices;
using Dollet.Core.Entities;
using Dollet.Core.Exceptions;
using Dollet.Helpers;
using Dollet.Models;
using Dollet.Pages.Popups;
using Dollet.ViewModels.Dtos;
using Dollet.ViewModels.Popups;

namespace Dollet.ViewModels.Accounts
{
    public partial class EditAccountPageViewModel(IAccountDomainService accountService, IUnitOfWork unitOfWork, IPopupService popupService) : ObservableObject, IQueryAttributable
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPopupService _popupService = popupService;
        private readonly IAccountDomainService _accountService = accountService;
        private static Users CurrentUser { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Account = (AccountDto)query["Account"];
        }

        [ObservableProperty]
        ObservableRangeCollection<string> _icons = [], _colors = [], _currencies = [];

        [ObservableProperty]
        AccountDto _account;

        [ObservableProperty]
        string _selectedCurrency;

        [ObservableProperty]
        bool _isDefault;

        [ObservableProperty]
        string _username;

        [ObservableProperty]
        string _password;

        public ObservableRangeCollection<CategoryDto> Categories { get; } = [];

        [RelayCommand]
        async Task Appearing()
        {
            Icons.ReplaceRange(Defined.Icons);
            Colors.ReplaceRange(Defined.Colors);

            var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();
            Currencies.ReplaceRange(currencies.Select(x => x.Code).ToList());
            CurrentUser = _unitOfWork.GetApplicationContext();

            SelectedCurrency = Account.Currency;
            IsDefault = Account.IsDefault;
            Username = Account.Username;
            Password = Account.Password;

            var categories = await _unitOfWork.AccountCategoryRepository.GetCategoriesByAccountIdAsync(Account.Id);
            var allCategoriesExpenses = await _unitOfWork.CategoryRepository.GetAllAsync();

            foreach (var category in allCategoriesExpenses)
            {
                var isSelected = categories.Any(c => c.CategoryId == category.Id);

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Color = category.Color,
                    Icon = category.Icon,
                    Name = category.Name,
                    IsSelected = isSelected
                };

                Categories.Add(categoryDto);
            }
        }

        [RelayCommand]
        async Task EditAccount()
        {
            Account.Currency = SelectedCurrency;

            var editedAccount = MapToEntity(Account);

            try
            {
                if (await _accountService.EditAsync(editedAccount, IsDefault))
                {
                    var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();

                    foreach (var item in popupViewModel.GetCacheData())
                    {
                        await _unitOfWork.AccountCategoryRepository.RemoveCategoryFromAccountAsync(Account.Id, item.Key);
                        await _unitOfWork.AccountCategoryRepository.AddCategoryToAccountAsync(Account.Id, item.Key, item.Value.Budget);
                        var success = await _unitOfWork.CommitAsync();
                    }

                    await Toast
                        .Make("Edited successfully", ToastDuration.Long, 14)
                        .Show();

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Toast
                        .Make("Something went wrong...", ToastDuration.Long, 14)
                        .Show();
                }
            }
            catch (Exception ex) when (ex is IDolletDomainException)
            {
                await Toast
                    .Make(ex.Message, ToastDuration.Long, 14)
                    .Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        async Task Delete()
        {
            var result = (ConfirmResult)await _popupService.ShowPopupAsync<ConfirmPopupViewModel>();

            if (result.ConfirmedForce)
                await ExecuteDelete(true);

            else if (result.Confirmed)
                await ExecuteDelete();
        }

        async Task ExecuteDelete(bool force = false)
        {
            try
            {
                await _accountService.DeleteAsync(MapToEntity(Account), force);

                var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();

                foreach (var item in popupViewModel.GetCacheData())
                {
                    await _unitOfWork.AccountCategoryRepository.RemoveCategoryFromAccountAsync(Account.Id, item.Key);
                    var success = await _unitOfWork.CommitAsync();
                }

                await Toast
                    .Make("Deleted", ToastDuration.Long, 14)
                    .Show();

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex) when (ex is IDolletDomainException)
            {
                await Toast
                    .Make(ex.Message, ToastDuration.Long, 14)
                    .Show();
            }
        }

        private static Account MapToEntity(AccountDto accountDto)
        {
            var account = new Account
            {
                Id = accountDto.Id,
                Amount = accountDto.Amount,
                Name = accountDto.Name,
                Description = accountDto.Description,
                Icon = accountDto.Icon,
                Color = accountDto.Color,
                Currency = accountDto.Currency,
                IsHidden = accountDto.IsHidden,
                UserId = CurrentUser.Id,
                User = CurrentUser,
                Username = accountDto.Username,
                Password = accountDto.Password
            };

            if (accountDto.IsDefault)
                account.SetAsDefault();

            return account;
        }

        [RelayCommand]
        public async void SelectCategory(CategoryDto category)
        {
            category.IsSelected = !category.IsSelected;
            var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();
            if (category.IsSelected)
            {
                popupViewModel.SetSelectedCategory(category.Id, true, Account.Id);

                var popup = new CategorySelectedPopup
                {
                    BindingContext = popupViewModel
                };
                popupViewModel.Popup = popup;
                await Shell.Current.ShowPopupAsync(popup);
            }
            else
            {
                popupViewModel.UndoSelectedCategory(category.Id);
            }
        }
    }
}