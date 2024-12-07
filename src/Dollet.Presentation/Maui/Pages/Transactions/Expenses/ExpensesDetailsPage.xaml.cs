using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Expenses;

namespace Dollet.Pages.Transactions.Expenses;

public partial class ExpensesDetailsPage : ContentPage
{
    public ExpensesDetailsPage()
	{
		InitializeComponent();

        BindingContext = ServiceProviderHelper.GetService<ExpensesDetailsPageViewModel>();
	}
}