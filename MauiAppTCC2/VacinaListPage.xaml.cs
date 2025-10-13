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

        public VacinaListPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;

            // CONFIGURAR BOTÃO
            btnAddVacina.Clicked += OnAddVacinaClicked;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarDados();
        }

        private async Task CarregarDados()
        {
            try
            {
                // OBTER PET ID DOS PARÂMETROS
                if (BindingContext is System.Collections.Generic.Dictionary<string, object> parameters)
                {
                    if (parameters.ContainsKey("PetId"))
                    {
                        _petId = (int)parameters["PetId"];

                        // CARREGAR DADOS DO PET
                        var pet = await _database.GetPetAsync(_petId);
                        if (pet != null)
                        {
                            _petNome = pet.Nome;
                            lblTitulo.Text = $"?? Vacinas";
                            lblPetNome.Text = pet.Nome;
                        }
                    }
                }

                // CARREGAR VACINAS
                await CarregarVacinas();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao carregar dados: {ex.Message}", "OK");
            }
        }

        private async Task CarregarVacinas()
        {
            try
            {
                var vacinas = await _database.GetVacinasByPetAsync(_petId);
                vacinasCollection.ItemsSource = vacinas;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao carregar vacinas: {ex.Message}", "OK");
            }
        }

        private async void OnAddVacinaClicked(object sender, System.EventArgs e)
        {
            await DisplayAlert("Adicionar Vacina",
                $"Funcionalidade para adicionar vacina ao {_petNome} será implementada em breve!", "OK");
        }
    }
}