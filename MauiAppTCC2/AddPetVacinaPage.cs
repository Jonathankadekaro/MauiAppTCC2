using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public class AddPetVacinaPage : ContentPage
    {
        private readonly DatabaseContext _database;
        private readonly int _petId;
        private readonly string _petNome;

        private Entry entryNome;
        private Entry entryFabricante;
        private Entry entryLote;
        private DatePicker dateAplicacao;
        private DatePicker dateValidade;
        private Picker pickerDose;
        private Entry entryVeterinario;
        private Entry entryClinica;

        public AddPetVacinaPage(DatabaseContext database, int petId, string petNome)
        {
            _database = database;
            _petId = petId;
            _petNome = petNome;
            SetupPage();
        }

        private void SetupPage()
        {
            Title = $"Vacina - {_petNome}";

            // ✅ CRIAR CONTROLES
            var lblTitulo = new Label
            {
                Text = $"Cadastrar Vacina para {_petNome}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            // Nome da Vacina
            entryNome = new Entry { Placeholder = "Ex: Antirrábica, V8, V10..." };

            // Fabricante
            entryFabricante = new Entry { Placeholder = "Fabricante da vacina" };

            // Lote
            entryLote = new Entry { Placeholder = "Número do lote" };

            // Data Aplicação
            dateAplicacao = new DatePicker
            {
                Date = DateTime.Now,
                Format = "dd/MM/yyyy"
            };

            // Data Validade
            dateValidade = new DatePicker
            {
                Date = DateTime.Now.AddYears(1),
                Format = "dd/MM/yyyy"
            };

            // Dose
            pickerDose = new Picker { Title = "Selecione a dose" };
            pickerDose.Items.Add("1ª Dose");
            pickerDose.Items.Add("2ª Dose");
            pickerDose.Items.Add("3ª Dose");
            pickerDose.Items.Add("Reforço Anual");
            pickerDose.Items.Add("Dose Única");

            // Veterinário
            entryVeterinario = new Entry { Placeholder = "Nome do veterinário" };

            // Clínica
            entryClinica = new Entry { Placeholder = "Nome da clínica" };

            // ✅ BOTÕES
            var btnSalvar = new Button
            {
                Text = "💾 Salvar Vacina",
                BackgroundColor = Color.FromArgb("#4CAF50"),
                TextColor = Colors.White,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HeightRequest = 55,
                CornerRadius = 12,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(0, 10, 0, 0)
            };
            btnSalvar.Clicked += BtnSalvar_Clicked;

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                HeightRequest = 50,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Fill
            };
            btnCancelar.Clicked += BtnCancelar_Clicked;

            // ✅ LAYOUT CORRIGIDO - CORES CONVERTIDAS
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 12,
                    Children = {
                        lblTitulo,

                        // 🔥 CORREÇÃO AQUI: "#333" → Color.FromArgb("#333333")
                        new Label { Text = "Nome da Vacina:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        entryNome,

                        new Label { Text = "Fabricante:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        entryFabricante,

                        new Label { Text = "Lote:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        entryLote,

                        new Label { Text = "Data de Aplicação:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        dateAplicacao,

                        new Label { Text = "Data de Validade:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        dateValidade,

                        new Label { Text = "Dose:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        pickerDose,

                        new Label { Text = "Veterinário:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        entryVeterinario,

                        new Label { Text = "Clínica:", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#333333") },
                        entryClinica,

                        btnSalvar,
                        btnCancelar
                    }
                }
            };
        }

        private async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            // ✅ VALIDAÇÃO
            if (string.IsNullOrWhiteSpace(entryNome.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe o nome da vacina.", "OK");
                return;
            }

            if (pickerDose.SelectedItem == null)
            {
                await DisplayAlert("Erro", "Por favor, selecione a dose.", "OK");
                return;
            }

            try
            {
                // ✅ CRIAR VACINA
                var vacina = new VacinaPet
                {
                    PetId = _petId,
                    Nome = entryNome.Text.Trim(),
                    Fabricante = entryFabricante.Text?.Trim() ?? "Não informado",
                    Lote = entryLote.Text?.Trim() ?? "Não informado",
                    DataAplicacao = dateAplicacao.Date,
                    DataValidade = dateValidade.Date,
                    Dose = pickerDose.SelectedItem.ToString(),
                    VeterinarioResponsavel = entryVeterinario.Text?.Trim() ?? "Não informado",
                    Clinica = entryClinica.Text?.Trim() ?? "Não informado",
                    NotificacaoAgendada = false
                };

                // ✅ SALVAR
                await _database.SaveVacinaAsync(vacina);
                await DisplayAlert("Sucesso", $"Vacina {vacina.Nome} salva com sucesso!", "OK");

                // ✅ VOLTAR PARA LISTA DE VACINAS
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar vacina: {ex.Message}", "OK");
            }
        }

        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}