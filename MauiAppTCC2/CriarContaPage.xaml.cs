using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class CriarContaPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public CriarContaPage(DatabaseContext database)
        {
            InitializeComponent();
            _database = database;
        }

        private async void OnCriarContaClicked(object sender, EventArgs e)
        {
            try
            {
                var nome = entryNome.Text?.Trim();
                var email = entryEmail.Text?.Trim();
                var senha = entrySenha.Text;
                var confirmarSenha = entryConfirmarSenha.Text;

                // ? VALIDAÇÕES
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(confirmarSenha))
                {
                    await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                    return;
                }

                if (senha.Length < 6)
                {
                    await DisplayAlert("Erro", "A senha deve ter no mínimo 6 caracteres.", "OK");
                    return;
                }

                if (senha != confirmarSenha)
                {
                    await DisplayAlert("Erro", "As senhas não coincidem.", "OK");
                    return;
                }

                // ? VERIFICAR SE EMAIL JÁ EXISTE
                var usuarioExistente = await _database.GetUsuarioByEmailAsync(email);
                if (usuarioExistente != null)
                {
                    await DisplayAlert("Erro", "Este email já está cadastrado.", "OK");
                    return;
                }

                // ? CRIAR NOVO USUÁRIO
                var novoUsuario = new Usuario
                {
                    Nome = nome,
                    Email = email,
                    Senha = senha
                };

                await _database.SaveUsuarioAsync(novoUsuario);
                await DisplayAlert("Sucesso", "Conta criada com sucesso! Faça login para continuar.", "OK");

                // ? VOLTAR PARA LOGIN
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao criar conta: {ex.Message}", "OK");
            }
        }

        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}