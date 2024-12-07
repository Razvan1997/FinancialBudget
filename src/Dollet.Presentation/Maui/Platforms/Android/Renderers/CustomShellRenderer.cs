using Android.Content;
using Dollet.Platforms.Android.Trackers;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Dollet.PlatformSpecifics.Renderers
{
    internal partial class CustomShellRenderer(Context context) : ShellRenderer(context)
    {
        protected override IShellTabLayoutAppearanceTracker CreateTabLayoutAppearanceTracker(ShellSection shellSection)
        {
            return new CustomTabLayoutAppearanceTracker(this);
        }
    }
}
