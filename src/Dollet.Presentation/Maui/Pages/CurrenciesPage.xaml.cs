using Dollet.Helpers;
using Dollet.ViewModels;

namespace Dollet.Pages;

public partial class CurrenciesPage : ContentPage
{
	public CurrenciesPage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<CurrenciesPageViewModel>();
	}
}