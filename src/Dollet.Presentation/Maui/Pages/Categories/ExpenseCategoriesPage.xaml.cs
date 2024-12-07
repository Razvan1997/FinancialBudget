using Dollet.ViewModels.Categories;

namespace Dollet.Pages.Categories;

public partial class ExpenseCategoriesPage : ContentPage
{
	public ExpenseCategoriesPage(ExpenseCategoriesPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }
}