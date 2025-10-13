using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class VacinaListPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private int _petId;
        private string _petNome;

        public VacinaListPage(DatabaseContext database, int petId, string petNome)
        {
            try
            {
                InitializeComponent();

                _database = database;
                _petId = petId;
                _petNome = petNome;

                // ? APENAS O TÍTULO PRINCIPAL - SEGUNDA LINHA JÁ ESTÁ FIXA NO XAML
                lblTitulo.Text = "Vacinas";
                // Removeu a linha: lblPetNome.Text = _petNome;

                btnAddVacina.Clicked += OnAddVacinaClicked;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? ERRO: {ex.Message}");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarVacinas();
        }

        private async Task CarregarVacinas()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"?? Carregando vacinas para pet ID: {_petId}");

                var vacinas = await _database.GetVacinasByPetAsync(_petId);

                // ? ATUALIZAR LISTA
                vacinasCollection.ItemsSource = vacinas;

                // ? ATUALIZAR CONTADOR (APENAS O NÚMERO)
                lblContador.Text = $"{vacinas.Count}";

                System.Diagnostics.Debug.WriteLine($"? {vacinas.Count} vacinas carregadas");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? ERRO ao carregar vacinas: {ex.Message}");
                await DisplayAlert("Erro", "Falha ao carregar vacinas", "OK");
            }
        }

        private async void OnAddVacinaClicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"?? Abrindo tela de adicionar vacina");

                var addVacinaPage = new AddPetVacinaPage(_database, _petId, _petNome);
                await Navigation.PushAsync(addVacinaPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"? ERRO: {ex.Message}");
                await DisplayAlert("Erro", "Falha ao abrir tela de vacina", "OK");
            }
        }
    }
}