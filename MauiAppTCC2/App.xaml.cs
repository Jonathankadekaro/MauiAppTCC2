using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class App : Application
    {
        // ✅ PROPRIEDADE GLOBAL PARA ACESSAR O USUÁRIO LOGADO
        public static Usuario UsuarioLogado { get; set; }

        public App()
        {
            InitializeComponent();

            // ✅ INICIA COM A TELA DE LOGIN
            var database = new DatabaseContext();
            MainPage = new NavigationPage(new LoginPage(database));
        }
    }
}