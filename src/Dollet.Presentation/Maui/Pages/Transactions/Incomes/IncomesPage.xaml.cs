using Dollet.Helpers;
using Dollet.ViewModels.Transactions.Incomes;

namespace Dollet.Pages.Transactions.Incomes
{
    public partial class IncomesPage : ContentPage
    {
        public IncomesPage()
        {
            InitializeComponent();

            BindingContext = ServiceProviderHelper.GetService<IncomesPageViewModel>();
        }
    }
}