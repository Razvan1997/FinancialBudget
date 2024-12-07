using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Expenses;

namespace Dollet.Pages.Transactions.Expenses;

public partial class EditExpensePage : ContentPage
{
	public EditExpensePage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<EditExpensePageViewModel>();
	}
}