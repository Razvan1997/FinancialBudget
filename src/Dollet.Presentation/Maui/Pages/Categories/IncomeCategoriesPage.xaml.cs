using Dollet.Helpers;
using Dollet.ViewModels.Categories;

namespace Dollet.Pages.Categories;

public partial class IncomeCategoriesPage : ContentPage
{
	public IncomeCategoriesPage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<IncomeCategoriesPageViewModel>();
	}
}