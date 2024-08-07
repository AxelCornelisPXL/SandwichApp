using Kwops.Mobile;
using Kwops.Mobile.Services;
using Kwops.Mobile.Services.Backend;
using KWops.Mobile.Services.Backend;
using Kwops.Mobile.Services.Identity;
using Kwops.Mobile.Settings;
using Kwops.Mobile.ViewModels;
using Kwops.Mobile.Views;
using AppBuilderExtensions = CommunityToolkit.Maui.AppBuilderExtensions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        AppBuilderExtensions.UseMauiCommunityToolkit(builder
                .UseMauiApp<App>())
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        RegisterDependencies(builder.Services);

        return builder.Build();
    }

    private static void RegisterDependencies(IServiceCollection services)
    {
        // Views
        services.AddTransient<LoginPage>();

        // ViewModels
        services.AddTransient<LoginViewModel>();

        // Services
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<INavigationService, NavigationService>();
        services.AddTransient<IToastService, ToastService>();

        // Other
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<IAppSettings, DevAppSettings>();

        //no clue if we need this, but I added it just in case
        services.AddSingleton<IBackendService, BackendService>();
        services.AddSingleton<ITeamsService, TeamsService>();
    }
}