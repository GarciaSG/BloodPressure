using BloodPressure.Maui.Helpers;
using BloodPressure.Maui.Services;
using BloodPressure.Maui.ViewModels;
using BloodPressure.Maui.Views;
using BloodPressureApp.Core.Validators;
using Microsoft.Extensions.Logging;

namespace BloodPressure.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient(ApiConfig.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(ApiConfig.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
        });

        builder.Services.AddTransient<BloodPressureCreateValidator>();
        builder.Services.AddTransient<BloodPressureUpdateValidator>();
        builder.Services.AddTransient<CreateUserValidator>();

        builder.Services.AddSingleton<BloodPressureApiService>();
        builder.Services.AddSingleton<BloodPressureViewModel>();
        builder.Services.AddSingleton<UserService>();

        builder.Services.AddTransient<BloodPressureListPage>();
        builder.Services.AddTransient<BloodPressureFormPage>();

        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<CreateUserView>();

        builder.Services.AddSingleton<UserViewModel>();

        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}