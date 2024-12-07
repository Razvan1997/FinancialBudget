using Dollet.Helpers;
using Dollet.ViewModels;

namespace Dollet.Pages;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<LoadingPageViewModel>();
	}
}