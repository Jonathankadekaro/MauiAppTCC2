using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2.ViewModels
{
    public class PetViewModel : BaseViewModel
    {
        private readonly DatabaseContext _database;
        private Pet _selectedPet;

        public ObservableCollection<Pet> Pets { get; } = new();
        public ObservableCollection<VacinaPet> Vacinas { get; } = new();

        public Pet SelectedPet
        {
            get => _selectedPet;
            set
            {
                SetProperty(ref _selectedPet, value);
                OnPropertyChanged(nameof(IsPetSelected));
            }
        }

        public bool IsPetSelected => SelectedPet != null;

        public ICommand LoadPetsCommand { get; }
        public ICommand AddPetCommand { get; }
        public ICommand DeletePetCommand { get; }
        public ICommand ShowVacinasCommand { get; }
        public ICommand RefreshCommand { get; }

        // ✅ CONSTRUTOR PÚBLICO SEM PARÂMETROS (PARA XAML)
        public PetViewModel() : this(new DatabaseContext())
        {
        }

        // ✅ CONSTRUTOR COM PARÂMETROS (PARA INJEÇÃO)
        public PetViewModel(DatabaseContext database)
        {
            _database = database;

            LoadPetsCommand = new Command(async () => await LoadPetsAsync());
            AddPetCommand = new Command(async () => await AddPetAsync());
            DeletePetCommand = new Command<Pet>(async (pet) => await DeletePetAsync(pet));
            ShowVacinasCommand = new Command<int>(async (petId) => await ShowVacinasAsync(petId));
            RefreshCommand = new Command(async () => await LoadPetsAsync());

            Task.Run(async () => await LoadPetsAsync());
        }

        private async Task LoadPetsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var pets = await _database.GetPetsAsync();

                Pets.Clear();
                foreach (var pet in pets)
                    Pets.Add(pet);

                // ✅ DEBUG: MOSTRA QUANTOS PETS FORAM CARREGADOS
                System.Diagnostics.Debug.WriteLine($"✅ Pets carregados: {pets.Count}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao carregar pets: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddPetAsync()
        {
            try
            {
                // ✅ USA A MESMA INSTÂNCIA DO DATABASE
                var addPetPage = new AddPetPage(_database);

                // ✅ EVENTO QUE GARANTE A ATUALIZAÇÃO
                addPetPage.Disappearing += async (sender, e) =>
                {
                    await LoadPetsAsync(); // ✅ RECARREGA A LISTA
                };

                await Application.Current.MainPage.Navigation.PushAsync(addPetPage);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha: {ex.Message}", "OK");
            }
        }

        private async Task DeletePetAsync(Pet pet)
        {
            if (pet == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Exclusão",
                $"Tem certeza que deseja excluir {pet.Nome} e todas as suas vacinas?",
                "Sim", "Não");

            if (confirm)
            {
                try
                {
                    await _database.DeletePetAsync(pet);
                    await LoadPetsAsync(); // ✅ RECARREGA A LISTA
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Pet excluído com sucesso", "OK");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao excluir pet: {ex.Message}", "OK");
                }
            }
        }

        private async Task ShowVacinasAsync(int petId)
        {
            try
            {
                var parameters = new Dictionary<string, object> { { "PetId", petId } };
                await Shell.Current.GoToAsync(nameof(VacinaListPage), parameters);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao abrir página de vacinas: {ex.Message}", "OK");
            }
        }

        public async Task LoadVacinasByPetAsync(int petId)
        {
            try
            {
                Vacinas.Clear();
                var vacinas = await _database.GetVacinasByPetAsync(petId);
                foreach (var vacina in vacinas)
                    Vacinas.Add(vacina);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao carregar vacinas: {ex.Message}", "OK");
            }
        }
    }
}