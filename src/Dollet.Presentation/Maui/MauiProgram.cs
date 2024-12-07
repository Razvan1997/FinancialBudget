using CommunityToolkit.Maui;
using Dollet.Core;
using Dollet.Infrastructure;
using Dollet.PlatformSpecifics;
using Dollet.PlatformSpecifics.Handlers;
using Dollet.PlatformSpecifics.Renderers;
using Microcharts.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.LifecycleEvents;
using System.Reflection;

namespace Dollet
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("VactorySans-Regular.ttf", "VactorySansRegular");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                })
                .UseMicrocharts()
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                    var platformHandlers = new Dictionary<DevicePlatform, Action>
                    {
                        { DevicePlatform.Android, ConfigureAndroidHandlers(handlers) }
                    };

                    if (platformHandlers.TryGetValue(DeviceInfo.Platform, out var configureHandlers))
                    {
                        configureHandlers();
                    }
                    else return;
                })
                .ConfigureLifecycleEvents(ConfigureEvents);

            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("Maui.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }

            builder.Services.AddPresentation();
            builder.Services.AddCore();
            builder.Services.AddInfrastructure(builder.Configuration);

            return builder.Build();
        }

        private static Action ConfigureAndroidHandlers(IMauiHandlersCollection handlers) => () =>
        {
            handlers.AddHandler(typeof(Shell), typeof(CustomShellRenderer));
            handlers.AddHandler(typeof(Picker), typeof(CustomPickerHandler));
            handlers.AddHandler(typeof(DatePicker), typeof(CustomDatePickerHandler));
        };

        private static void ConfigureEvents(ILifecycleBuilder events)
        {
            events.AddAndroid(android =>
            {
                android.OnCreate((activity, bundle) =>
                {
                    BaseActivity.Set(activity);
                    Application.Current.RequestedThemeChanged += AppThemeChanged;
                });
            });
        }

        private static void AppThemeChanged(object sender, AppThemeChangedEventArgs e)
            => BaseActivity.SetNavigationBarColor();
    }
}