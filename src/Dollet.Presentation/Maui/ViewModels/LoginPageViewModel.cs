using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dollet.Core.Abstractions;
using Dollet.Helpers;
using Dollet.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.ViewModels
{
    public partial class LoginPageViewModel(IUnitOfWork unitOfWork) : ObservableObject
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [RelayCommand]
        async Task Appearing()
        {
            var users = await _unitOfWork.UserRepository.GetAsync(1);
            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            appShellViewModel.IsLogoutVisible = false;

            var currentContext = unitOfWork.GetApplicationContext();

            if(currentContext != null)
            {
                var stack = Shell.Current.Navigation.NavigationStack.ToArray();
                for (int i = stack.Length - 1; i > 0; i--)
                {
                    Shell.Current.Navigation.RemovePage(stack[i]);
                }
            }
        }

        [RelayCommand]
        async Task Login()
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                if (Username == "Admin" && Password == "Admin")
                {
                    var currentUser = await _unitOfWork.UserRepository.GetAsync(1);

                    _unitOfWork.SetApplicationContext(currentUser);
                    SetFlyoutItemVisibility("Categorii", true);
                    SetFlyoutItemVisibility("Monede", true);
                    SetFlyoutItemVisibility("Setari", true);
                    SetFlyoutItemVisibility("Portofele", true);
                    await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                    var context = _unitOfWork.GetApplicationContext();
                }
                else
                {
                    var normalUser = await _unitOfWork.UserRepository.GetByUsernameAndPasswordAsync(Username, Password);

                    if(normalUser != null)
                    {
                        _unitOfWork.SetApplicationContext(normalUser);

                        SetFlyoutItemVisibility("Categorii", false);
                        SetFlyoutItemVisibility("Monede", false);
                        SetFlyoutItemVisibility("Setari", false);
                        SetFlyoutItemVisibility("Portofele", false);

                        await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                    }
                }
            }
            else
            {
                // Logic pentru câmpuri goale
            }
        }

        private void SetFlyoutItemVisibility(string title, bool isVisible)
        {
            var shell = Shell.Current;

            var flyoutItem = shell.Items.FirstOrDefault(item => item.Title == title);

            if (flyoutItem != null)
            {
                flyoutItem.SetValue(Shell.FlyoutItemIsVisibleProperty, isVisible);
            }
        }
    }
}
