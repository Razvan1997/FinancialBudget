using Android.App;

namespace Dollet.PlatformSpecifics
{
    internal partial class BaseActivity
    {
        public static Activity CurrentActivity { get; private set; }

        public static void Set(Activity activity) => CurrentActivity = activity;
        public static void SetNavigationBarColor()
        {
            var currentTheme = Microsoft.Maui.Controls.Application.Current.UserAppTheme;
            var color = currentTheme == AppTheme.Dark ? Android.Graphics.Color.Rgb(31, 31, 31) : Android.Graphics.Color.White;
            
            CurrentActivity?.Window.SetNavigationBarColor(color);
        }
    }
}