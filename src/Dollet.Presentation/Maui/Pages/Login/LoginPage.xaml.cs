using Dollet.Core.Abstractions;
using Dollet.Helpers;
using Dollet.ViewModels;

namespace Dollet.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
	{
		InitializeComponent();
        BindingContext = ServiceProviderHelper.GetService<LoginPageViewModel>();
    }

    private async void OnTermsTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Termeni și Condiții", "Aceasta este o aplicație demo. Citește termenii și condițiile complete.", "OK");
    }
}