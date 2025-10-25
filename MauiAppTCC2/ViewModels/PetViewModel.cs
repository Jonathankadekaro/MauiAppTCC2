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

        public PetViewModel() : this(new DatabaseContext())
        {
        }

        public PetViewModel(DatabaseContext database)
        {
            _database = database;

            LoadPetsCommand = new Command(async () => await LoadPetsAsync());
            AddPetCommand = new Command(async () => await AddPetAsync());
            DeletePetCommand = new Command<Pet>(async (pet) => await DeletePetAsync(pet));
            ShowVacinasCommand = new Command<int>(async (petId) => await ShowVacinasAsync(petId));
            RefreshCommand = new Command(async () => await LoadPetsAsync());
        }

        private async Task LoadPetsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // ✅ CARREGA APENAS OS PETS DO USUÁRIO LOGADO
                var pets = await _database.GetPetsByUsuarioAsync(App.UsuarioLogado.Id);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Pets.Clear();
                    foreach (var pet in pets)
                        Pets.Add(pet);
                });

                System.Diagnostics.Debug.WriteLine($"✅ Pets carregados: {pets.Count} para usuário {App.UsuarioLogado.Nome}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erro ao carregar pets: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task AddPetAsync()
        {
            try
            {
                var addPetPage = new AddPetPage(_database);
                addPetPage.Disappearing += async (sender, e) =>
                {
                    await LoadPetsAsync();
                };

                await Application.Current.MainPage.Navigation.PushAsync(addPetPage);
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"Falha: {ex.Message}", "OK");
                }
            }
        }

        private async Task DeletePetAsync(Pet pet)
        {
            if (pet == null) return;

            if (Application.Current?.MainPage == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Exclusão",
                $"Tem certeza que deseja excluir {pet.Nome} e todas as suas vacinas?",
                "Sim", "Não");

            if (confirm)
            {
                try
                {
                    await _database.DeletePetAsync(pet);
                    await LoadPetsAsync();

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Pets.Remove(pet);
                    });

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
                System.Diagnostics.Debug.WriteLine($"🔍 1. Iniciando navegação...");

                // ✅ TESTE 1: Só criar a página (sem navegar)
                System.Diagnostics.Debug.WriteLine($"🔍 2. Criando VacinaListPage...");
                var vacinaListPage = new VacinaListPage(_database, petId, "Teste Pet");
                System.Diagnostics.Debug.WriteLine($"🔍 3. Página criada com sucesso!");

                // ✅ TESTE 2: Tentar navegar
                System.Diagnostics.Debug.WriteLine($"🔍 4. Iniciando navegação...");
                await Application.Current.MainPage.Navigation.PushAsync(vacinaListPage);
                System.Diagnostics.Debug.WriteLine($"🔍 5. Navegação concluída!");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERRO DETALHADO:");
                System.Diagnostics.Debug.WriteLine($"❌ Mensagem: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Tipo: {ex.GetType()}");
                System.Diagnostics.Debug.WriteLine($"❌ Local: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        public async Task LoadVacinasByPetAsync(int petId)
        {
            try
            {
                var vacinas = await _database.GetVacinasByPetAsync(petId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Vacinas.Clear();
                    foreach (var vacina in vacinas)
                        Vacinas.Add(vacina);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erro ao carregar vacinas: {ex.Message}");
            }
        }

        public async Task SearchPetsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                await LoadPetsAsync();
                return;
            }

            try
            {
                var pets = await _database.SearchPetsAsync(searchTerm);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Pets.Clear();
                    foreach (var pet in pets)
                        Pets.Add(pet);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erro na pesquisa: {ex.Message}");
            }
        }
    }
}