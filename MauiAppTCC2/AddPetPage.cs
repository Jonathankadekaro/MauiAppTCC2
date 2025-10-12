using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public class AddPetPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private Entry entryNome;
        private Picker pickerEspecie;
        private Entry entryRaca;
        private DatePicker dateNascimento;
        private Entry entryPeso;

        // ✅ RECEBA DatabaseContext POR INJEÇÃO
        public AddPetPage(DatabaseContext database)
        {
            _database = database;
            SetupPage();
        }

        private void SetupPage()
        {
            Title = "Adicionar Pet";

            // ✅ CRIAR CONTROLES
            var lblTitulo = new Label
            {
                Text = "Adicionar Novo Pet",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            entryNome = new Entry { Placeholder = "Nome do pet" };
            pickerEspecie = new Picker { Title = "Selecione a espécie" };
            pickerEspecie.Items.Add("Cachorro");
            pickerEspecie.Items.Add("Gato");
            pickerEspecie.Items.Add("Pássaro");
            pickerEspecie.Items.Add("Outro");

            entryRaca = new Entry { Placeholder = "Raça do pet" };
            dateNascimento = new DatePicker { Date = DateTime.Now.AddYears(-1) };
            entryPeso = new Entry { Placeholder = "0.0", Keyboard = Keyboard.Numeric };

            // ✅ BOTÕES
            var btnSalvar = new Button
            {
                Text = "Salvar Pet",
                BackgroundColor = Color.FromArgb("#2196F3"),
                TextColor = Colors.White,
                Margin = new Thickness(0, 10, 0, 0)
            };
            btnSalvar.Clicked += BtnSalvar_Clicked;

            var btnCancelar = new Button
            {
                Text = "Cancelar",
                BackgroundColor = Colors.LightGray
            };
            btnCancelar.Clicked += BtnCancelar_Clicked;

            // ✅ LAYOUT
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    Children = {
                        lblTitulo,
                        new Label { Text = "Nome:", FontAttributes = FontAttributes.Bold }, entryNome,
                        new Label { Text = "Espécie:", FontAttributes = FontAttributes.Bold }, pickerEspecie,
                        new Label { Text = "Raça:", FontAttributes = FontAttributes.Bold }, entryRaca,
                        new Label { Text = "Data Nascimento:", FontAttributes = FontAttributes.Bold }, dateNascimento,
                        new Label { Text = "Peso (kg):", FontAttributes = FontAttributes.Bold }, entryPeso,
                        btnSalvar,
                        btnCancelar
                    }
                }
            };
        }

        private async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryNome.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe o nome do pet.", "OK");
                return;
            }

            try
            {
                var pet = new Pet
                {
                    Nome = entryNome.Text.Trim(),
                    Especie = pickerEspecie.SelectedItem?.ToString() ?? "Não informado",
                    Raca = entryRaca.Text?.Trim() ?? "Não informado",
                    DataNascimento = dateNascimento.Date,
                    Peso = string.IsNullOrWhiteSpace(entryPeso.Text) ? 0 : double.Parse(entryPeso.Text),
                    DataCadastro = DateTime.Now
                };

                await _database.SavePetAsync(pet);
                await DisplayAlert("Sucesso", $"{pet.Nome} salvo com sucesso!", "OK");

                await Navigation.PopAsync(); // ✅ VOLTA PARA A LISTA
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar: {ex.Message}", "OK");
            }
        }

        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // ✅ VOLTA PARA A LISTA
        }
    }
}