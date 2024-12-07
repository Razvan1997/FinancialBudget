using CommunityToolkit.Maui.Views;
using Dollet.Helpers;
using Dollet.ViewModels.Popups;

namespace Dollet.Pages.Popups;

public partial class ConfirmPopup : Popup
{
	public ConfirmPopup()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<ConfirmPopupViewModel>();
    }
}