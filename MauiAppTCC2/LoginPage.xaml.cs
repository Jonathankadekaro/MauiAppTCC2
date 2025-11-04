using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using MauiAppTCC2.ViewModels; // ? ADICIONE ESTA LINHA
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public LoginPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var email = entryEmail.Text?.Trim();
                var senha = entrySenha.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                    return;
                }

                // Buscar usuário no banco
                var usuario = await _database.GetUsuarioByEmailAsync(email);

                if (usuario == null || usuario.Senha != senha)
                {
                    await DisplayAlert("Erro", "Email ou senha incorretos.", "OK");
                    return;
                }

                // Login bem-sucedido - salvar usuário logado
                App.UsuarioLogado = usuario;

                // ? CORREÇÃO: Criar MainPage com ViewModel
                var petViewModel = new PetViewModel(_database);
                var mainPage = new MainPage(petViewModel);
                await Navigation.PushAsync(mainPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha no login: {ex.Message}", "OK");
            }
        }

        private async void OnCriarContaClicked(object sender, EventArgs e)
        {
            // ? AGORA DESCOMENTADO E FUNCIONAL
            await Navigation.PushAsync(new CriarContaPage(_database));
        }
    }
}