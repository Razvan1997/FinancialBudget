using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Core.Abstractions.DomainServices;
using Dollet.Core.Abstractions.Repositories;
using Dollet.Core.Entities;
using Dollet.Helpers;
using Dollet.Pages;
using Dollet.Pages.Popups;
using Dollet.ViewModels.Dtos;
using Dollet.ViewModels.Popups;

namespace Dollet.ViewModels.Accounts
{
    public partial class AddAccountPageViewModel(IAccountDomainService accountService, IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IAccountDomainService _accountService = accountService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICategoryRepository _categoryRepository = unitOfWork.CategoryRepository;
        private readonly IAccountCategoryRepository _accountCategoryRepository = unitOfWork.AccountCategoryRepository;

        [ObservableProperty]
        ObservableRangeCollection<string> _icons = [], _colors = [], _currencies = [];
        public ObservableRangeCollection<CategoryDto> Categories { get; } = [];

        [ObservableProperty]
        decimal? _amount;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddAccountCommand))]
        string _name, _selectedIcon, _selectedColor, _selectedCurrency;
        
        [ObservableProperty]
        string _description = string.Empty;
        
        [ObservableProperty]
        bool _isHidden, _isDefault;

        [ObservableProperty]
        string _username = string.Empty;

        [ObservableProperty]
        string _password = string.Empty;

        [RelayCommand]
        async Task Appearing()
        {
            Icons.ReplaceRange(Defined.Icons);
            Colors.ReplaceRange(Defined.Colors);

            var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();
            Currencies.ReplaceRange(currencies.Select(x => x.Code).ToList());

            SelectedCurrency = currencies.FirstOrDefault(x => x.IsDefault)?.Code;
            IsDefault = !await _unitOfWork.AccountRepository.ExistsAsync();

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            Categories.ReplaceRange(categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
                Color = c.Color,
                IsSelected = false 
            }));

            var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();
            popupViewModel.Clear();
        }

        [RelayCommand]
        public async void SelectCategory(CategoryDto category)
        {
            category.IsSelected = !category.IsSelected;
            var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();
            if (category.IsSelected)
            {
                popupViewModel.SetSelectedCategory(category.Id, false);

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

        [RelayCommand(CanExecute = nameof(CanAddAccount))]
        async Task AddAccount()
        {
            var currentUser = _unitOfWork.GetApplicationContext();
            if (currentUser == null)
            {
                throw new InvalidOperationException("No user is currently logged in.");
            }
            try
            {
                var account = new Account()
                {
                    Amount = Amount ?? 0,
                    Description = Description,
                    Name = Name,
                    Icon = SelectedIcon,
                    Color = SelectedColor,
                    Currency = SelectedCurrency,
                    IsHidden = IsHidden,
                    UserId = currentUser.Id,
                    Username = Username,
                    Password = Password,
                };

                var exist = await _accountService.AccountAlreadyExist(account);

                if (!exist)
                {
                    var added = await _accountService.CreateAndGetIdAsync(new Account
                    {
                        Amount = Amount ?? 0,
                        Description = Description,
                        Name = Name,
                        Icon = SelectedIcon,
                        Color = SelectedColor,
                        Currency = SelectedCurrency,
                        IsHidden = IsHidden,
                        UserId = currentUser.Id,
                        Username = Username,
                        Password = Password
                    }, IsDefault);

                    if (added != null)
                    {
                        var createdUser = new Users() { Name = Username, Password = Password, Role = Core.Enums.UserType.Normal };
                        _unitOfWork.UserRepository.Add(createdUser);

                        var popupViewModel = ServiceProviderHelper.GetService<CategorySelectedPopupViewModel>();
                        popupViewModel.UpdateAccountIdForAllCategories(added.Value);

                        //todo add many to many account to categories

                        foreach (var item in popupViewModel.GetCacheData())
                        {
                            await _accountCategoryRepository.AddCategoryToAccountAsync((int)added, item.Key, item.Value.Budget);
                            var success = await _unitOfWork.CommitAsync();
                        }
                        await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Eroare", $"Utilizatorul de cont cu user-ul {Username}, exista deja !", "OK");
                }
            }
            catch(Exception ex)
            {

            }
        }

        bool CanAddAccount() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(SelectedIcon) &&
            !string.IsNullOrWhiteSpace(SelectedColor) &&
            !string.IsNullOrWhiteSpace(SelectedCurrency);
    }
}