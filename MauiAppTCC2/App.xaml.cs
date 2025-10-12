using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // ✅ CONFIGURAR SHELL COM ROTAS
            var shell = new Shell();

            // ✅ REGISTRAR ROTAS
            Routing.RegisterRoute(nameof(AddPetPage), typeof(AddPetPage));
            Routing.RegisterRoute(nameof(VacinaListPage), typeof(VacinaListPage));

            // ✅ PÁGINA INICIAL
            shell.CurrentItem = new ShellContent
            {
                Content = new MainPage()
            };

            MainPage = shell;
        }
    }
}