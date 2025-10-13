using Microsoft.Maui.Controls;
using MauiAppTCC2.ViewModels;
using MauiAppTCC2.Data;

namespace MauiAppTCC2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // ✅ SOLUÇÃO: CRIAR MANUALMENTE COM DEPENDÊNCIAS
            var database = new DatabaseContext();
            var viewModel = new PetViewModel(database);
            var mainPage = new MainPage(viewModel);

            MainPage = new NavigationPage(mainPage);
        }
    }
}