using Dollet.Core.Constants;
using Dollet.Pages;

namespace Dollet
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            _ = MaterialDesignIcons.Local_grocery_store;
            _ = MaterialDesignIcons.Sports_baseball;
            _ = MaterialDesignIcons.Restaurant;
        }
    }
}
