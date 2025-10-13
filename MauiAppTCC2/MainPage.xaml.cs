using MauiAppTCC2.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class MainPage : ContentPage
    {
        public MainPage(PetViewModel viewModel)
        {
            InitializeComponent();

            // ✅ BINDINGCONTEXT VIA INJEÇÃO
            BindingContext = viewModel;

            System.Diagnostics.Debug.WriteLine("✅ MainPage inicializada");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // ✅ USA O COMMAND PARA CARREGAR PETS
            if (BindingContext is PetViewModel viewModel)
            {
                if (viewModel.LoadPetsCommand.CanExecute(null))
                {
                    viewModel.LoadPetsCommand.Execute(null);
                }
            }

            System.Diagnostics.Debug.WriteLine("✅ OnAppearing executado");
        }
    }
}