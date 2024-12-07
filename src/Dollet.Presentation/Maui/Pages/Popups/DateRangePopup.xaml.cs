using CommunityToolkit.Maui.Views;
using Dollet.Helpers;
using Dollet.ViewModels.Popups;

namespace Dollet.Pages.Popups;

public partial class DateRangePopup : Popup
{
	public DateRangePopup()
	{
		InitializeComponent();

		BindingContext = ServiceProviderHelper.GetService<DateRangePopupViewModel>();
	}
}