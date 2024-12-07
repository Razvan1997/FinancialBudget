using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Incomes;

namespace Dollet.Pages.Transactions.Incomes;

public partial class AddIncomePage : ContentPage
{
	public AddIncomePage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<AddIncomePageViewModel>();
	}
}