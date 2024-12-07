using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Color = Android.Graphics.Color;

namespace Dollet.PlatformSpecifics.Handlers
{
    internal partial class CustomPickerHandler : PickerHandler
    {
        protected override void ConnectHandler(MauiPicker platformView)
        {
            base.ConnectHandler(platformView);

            platformView.Background = null;
            platformView.SetBackgroundColor(Color.Transparent);
            platformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
        }
    }
}