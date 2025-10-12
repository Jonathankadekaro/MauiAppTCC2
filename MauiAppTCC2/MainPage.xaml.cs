using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAddPetClicked(object sender, System.EventArgs e)
        {
            // ✅ EVENTO SIMPLES PARA TESTE
            DisplayAlert("Teste", "Botão funcionando!", "OK");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}