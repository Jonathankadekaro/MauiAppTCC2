using Microsoft.Extensions.Logging;
using MauiAppTCC2.Data;
using MauiAppTCC2.Services;
using MauiAppTCC2.ViewModels;

namespace MauiAppTCC2
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ✅ SERVICES - CONFIGURAÇÃO SQLITE
            builder.Services.AddSingleton<DatabaseContext>();

            // ✅ SE NÃO TEM NotificationService, COMENTE ESTA LINHA:
            // builder.Services.AddSingleton<INotificationService, NotificationService>();

            builder.Services.AddTransient<PetViewModel>();
            builder.Services.AddTransient<AddPetPage>();
            builder.Services.AddTransient<VacinaListPage>();
            builder.Services.AddTransient<AddPetVacinaPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<PetViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CriarContaPage>();

            return builder.Build();
        }
    }
}