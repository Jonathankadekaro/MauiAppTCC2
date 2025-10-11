using MauiAppTCC2.Services;

namespace MauiAppTCC2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseLocalNotification() // habilita notificações locais
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 🔹 Registrando NotificationService como Singleton
            builder.Services.AddSingleton<Services.INotificationService, NotificationService>();

            return builder.Build();
        }
    }
}