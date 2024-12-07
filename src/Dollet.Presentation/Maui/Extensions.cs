using CommunityToolkit.Maui;
using Dollet.Pages;
using Dollet.Pages.Accounts;
using Dollet.Pages.Categories;
using Dollet.Pages.Popups;
using Dollet.ViewModels;
using Dollet.ViewModels.Accounts;
using Dollet.ViewModels.Categories;
using Dollet.ViewModels.Popups;
using Dollet.ViewModels.Transactions.Expenses;
using Dollet.ViewModels.Transactions.Incomes;

namespace Dollet
{
    internal static class Extensions
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddPages();
            services.AddViewModels();
            services.AddPopups();

            return services;
        }

        private static IServiceCollection AddPages(this IServiceCollection services) =>
            services
                .AddTransient<WalletPage>()
                .AddTransient<AccountsPage>()
                .AddTransient<AddAccountPage>()
                .AddTransient<EditAccountPage>()
                .AddTransient<ExpenseCategoriesPage>()
                .AddTransient<InwestmentsPage>()
                .AddTransient<SettingsPage>()
                .AddTransient<LoginPage>();

        private static IServiceCollection AddViewModels(this IServiceCollection services) =>
            services
                .AddTransient<AppShellViewModel>()
                .AddTransient<WalletPageViewModel>()
                .AddTransient<AccountsPageViewModel>()
                .AddTransient<AddAccountPageViewModel>()
                .AddTransient<EditAccountPageViewModel>()
                .AddTransient<ExpenseCategoriesPageViewModel>()
                .AddTransient<IncomeCategoriesPageViewModel>()
                .AddTransient<ExpensesPageViewModel>()
                .AddTransient<AddExpensePageViewModel>()
                .AddTransient<EditExpensePageViewModel>()
                .AddTransient<ExpensesDetailsPageViewModel>()
                .AddTransient<IncomesPageViewModel>()
                .AddTransient<IncomesDetailsPageViewModel>()
                .AddTransient<AddIncomePageViewModel>()
                .AddTransient<EditIncomePageViewModel>()
                .AddTransient<CurrenciesPageViewModel>()
                .AddTransient<InwestmentsPageViewModel>()
                .AddTransient<SettingsPageViewModel>()
                .AddTransient<LoadingPageViewModel>()
                .AddTransient<LoginPageViewModel>();

        private static IServiceCollection AddPopups(this IServiceCollection services) => 
            services
                .AddTransientPopup<ConfirmPopup, ConfirmPopupViewModel>()
                .AddTransientPopup<DateRangePopup, DateRangePopupViewModel>()
                .AddSingleton<CategorySelectedPopup, CategorySelectedPopupViewModel>();
    }
}
