using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Incomes;

namespace Dollet.Pages.Transactions.Incomes;

public partial class IncomesDetailsPage : ContentPage
{
	public IncomesDetailsPage()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<IncomesDetailsPageViewModel>();
	}
}