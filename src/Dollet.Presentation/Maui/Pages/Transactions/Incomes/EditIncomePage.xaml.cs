using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Incomes;

namespace Dollet.Pages.Transactions.Incomes;

public partial class EditIncomePage : ContentPage
{
	public EditIncomePage()
	{
		InitializeComponent();
		
		BindingContext = ServiceProviderHelper.GetService<EditIncomePageViewModel>();
	}
}