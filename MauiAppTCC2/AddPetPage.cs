using MauiAppTCC2.Data;
using MauiAppTCC2.Models;

namespace MauiAppTCC2
{
    public class AddPetPage : ContentPage
    {
        private readonly DatabaseContext _database;

        public AddPetPage(DatabaseContext database)
        {
            _database = database;

            Title = "Adicionar Pet";

            var entryNome = new Entry { Placeholder = "Nome do pet" };
            var buttonSalvar = new Button { Text = "Salvar" };
            buttonSalvar.Clicked += async (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(entryNome.Text))
                {
                    var pet = new Pet { Nome = entryNome.Text };
                    await _database.SavePetAsync(pet);
                    await DisplayAlert("Sucesso", "Pet salvo!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            };

            Content = new StackLayout
            {
                Padding = 20,
                Children = { entryNome, buttonSalvar }
            };
        }
    }
}