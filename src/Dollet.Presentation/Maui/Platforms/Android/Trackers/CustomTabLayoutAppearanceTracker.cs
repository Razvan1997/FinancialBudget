using Google.Android.Material.Tabs;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace Dollet.Platforms.Android.Trackers
{
    internal class CustomTabLayoutAppearanceTracker(IShellContext shellContext) : ShellTabLayoutAppearanceTracker(shellContext)
    {
        public override void SetAppearance(TabLayout tabLayout, ShellAppearance appearance)
        {
            base.SetAppearance(tabLayout, appearance);

            tabLayout.TabMode = TabLayout.ModeFixed;
            tabLayout.TabGravity = TabLayout.GravityFill;
        }
    }
}
