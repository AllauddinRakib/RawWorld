namespace RawWorld.App;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using RawWorld.App.Pages;
using RawWorld.App.Services;
using RawWorld.App.ViewModels;

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

        // Services
        builder.Services.AddSingleton<ApiService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ProductListViewModel>();
        builder.Services.AddTransient<CartViewModel>();
        builder.Services.AddTransient<OrderListViewModel>();
        builder.Services.AddTransient<AdminViewModel>();
        builder.Services.AddTransient<PaymentViewModel>();
        builder.Services.AddTransient<AdminProductViewModel>();
        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ProductListPage>();
        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<OrderListPage>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<PaymentPage>();
        builder.Services.AddTransient<AdminProductPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}