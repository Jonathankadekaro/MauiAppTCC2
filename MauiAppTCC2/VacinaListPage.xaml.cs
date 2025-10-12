using MauiAppTCC2.Data;

namespace MauiAppTCC2
{
    public partial class VacinaListPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public VacinaListPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;

            // Configurar controles diretamente no construtor
            lblTitulo.Text = "Carregando...";
            lblMensagem.Text = "Aguarde...";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (BindingContext is Dictionary<string, object> parameters)
                {
                    if (parameters.ContainsKey("PetId"))
                    {
                        int petId = (int)parameters["PetId"];
                        var pet = await _database.GetPetAsync(petId);

                        if (pet != null)
                        {
                            lblTitulo.Text = $"Vacinas de {pet.Nome}";
                            lblMensagem.Text = $"Pet ID: {petId} carregado com sucesso!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = $"Erro: {ex.Message}";
            }
        }

        private async void BtnVoltar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}