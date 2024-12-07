using Dollet.Helpers;
using Dollet.Pages;
using Dollet.Pages.Accounts;
using Dollet.Pages.Categories;
using Dollet.Pages.Transactions.Expenses;
using Dollet.Pages.Transactions.Incomes;
using Dollet.ViewModels;

namespace Dollet
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            BindingContext = ServiceProviderHelper.GetService<AppShellViewModel>();
            this.FlyoutBehavior = FlyoutBehavior.Disabled;
            Routing.RegisterRoute("WalletPage", typeof(WalletPage));

            Routing.RegisterRoute("AccountsPage", typeof(AccountsPage));
            Routing.RegisterRoute("AddAccountPage", typeof(AddAccountPage));
            Routing.RegisterRoute("EditAccountPage", typeof(EditAccountPage));
            
            Routing.RegisterRoute("IncomesPage", typeof(IncomesPage));
            Routing.RegisterRoute("IncomesDetailsPage", typeof(IncomesDetailsPage));
            Routing.RegisterRoute("AddIncomePage", typeof(AddIncomePage));
            Routing.RegisterRoute("EditIncomePage", typeof(EditIncomePage));

            Routing.RegisterRoute("ExpensesPage", typeof(ExpensesPage));
            Routing.RegisterRoute("ExpensesDetailsPage", typeof(ExpensesDetailsPage));
            Routing.RegisterRoute("AddExpensePage", typeof(AddExpensePage));
            Routing.RegisterRoute("EditExpensePage", typeof(EditExpensePage));

            Routing.RegisterRoute("CategoriesPage", typeof(ExpenseCategoriesPage));
            Routing.RegisterRoute("IncomeCategoriesPage", typeof(IncomeCategoriesPage));

            Routing.RegisterRoute("CurrenciesPage", typeof(CurrenciesPage));
            Routing.RegisterRoute("InwestmentsPage", typeof(InwestmentsPage));
            Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("LoadingPage", typeof(LoadingPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        }

        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            if (e.Current.Location.OriginalString.Contains("LoadingPage"))
            {
                this.FlyoutBehavior = FlyoutBehavior.Disabled;
            }
            else
            {
                this.FlyoutBehavior = FlyoutBehavior.Flyout;
            }
            if (e.Current.Location.OriginalString.Contains("LogoutPage"))
            {
                var viewModel = BindingContext as AppShellViewModel;
                viewModel?.LogoutCommand.Execute(null);
            }
        }
    }
}
