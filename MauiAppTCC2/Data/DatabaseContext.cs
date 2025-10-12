using MauiAppTCC2.Models;
using System.Collections.ObjectModel;

namespace MauiAppTCC2.Data
{
    public class DatabaseContext
    {
        private ObservableCollection<Pet> _pets = new ObservableCollection<Pet>();
        private ObservableCollection<VacinaPet> _vacinas = new ObservableCollection<VacinaPet>(); // ✅ USANDO VacinaPet
        private int _nextPetId = 1;
        private int _nextVacinaId = 1;

        public DatabaseContext()
        {
            _pets.Add(new Pet { Id = _nextPetId++, Nome = "Rex", Especie = "Cachorro", Raca = "Vira-lata", DataNascimento = DateTime.Now.AddYears(-2) });
            _pets.Add(new Pet { Id = _nextPetId++, Nome = "Mimi", Especie = "Gato", Raca = "Siamês", DataNascimento = DateTime.Now.AddYears(-1) });
        }

        public async Task<List<Pet>> GetPetsAsync()
        {
            await Task.Delay(100);
            return _pets.OrderBy(p => p.Nome).ToList();
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            await Task.Delay(100);
            return _pets.FirstOrDefault(p => p.Id == id);
        }

        public async Task<int> SavePetAsync(Pet pet)
        {
            await Task.Delay(100);

            if (pet.Id == 0)
            {
                pet.Id = _nextPetId++;
                _pets.Add(pet);
                return 1;
            }
            else
            {
                var existing = _pets.FirstOrDefault(p => p.Id == pet.Id);
                if (existing != null)
                {
                    _pets.Remove(existing);
                    _pets.Add(pet);
                }
                return 1;
            }
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            await Task.Delay(100);

            var vacinasPet = _vacinas.Where(v => v.PetId == pet.Id).ToList();
            foreach (var vacina in vacinasPet)
            {
                _vacinas.Remove(vacina);
            }

            _pets.Remove(pet);
            return 1;
        }

        public async Task<List<VacinaPet>> GetVacinasByPetAsync(int petId) // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);
            return _vacinas.Where(v => v.PetId == petId)
                          .OrderByDescending(v => v.DataAplicacao)
                          .ToList();
        }

        public async Task<int> SaveVacinaAsync(VacinaPet vacina) // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);

            if (vacina.Id == 0)
            {
                vacina.Id = _nextVacinaId++;
                _vacinas.Add(vacina);
                return 1;
            }
            else
            {
                var existing = _vacinas.FirstOrDefault(v => v.Id == vacina.Id);
                if (existing != null)
                {
                    _vacinas.Remove(existing);
                    _vacinas.Add(vacina);
                }
                return 1;
            }
        }

        public async Task<int> DeleteVacinaAsync(VacinaPet vacina) // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);
            _vacinas.Remove(vacina);
            return 1;
        }

        public async Task<List<VacinaPet>> GetAllVacinasAsync() // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);
            return _vacinas.ToList();
        }

        public async Task<List<VacinaPet>> GetProximasVacinasVencerAsync() // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);
            var dataLimite = DateTime.Now.AddDays(30);
            return _vacinas.Where(v => v.DataValidade <= dataLimite &&
                                      v.DataValidade >= DateTime.Now)
                          .OrderBy(v => v.DataValidade)
                          .ToList();
        }

        public async Task<List<VacinaPet>> GetVacinasVencidasAsync() // ✅ USANDO VacinaPet
        {
            await Task.Delay(100);
            return _vacinas.Where(v => v.DataValidade < DateTime.Now)
                          .OrderBy(v => v.DataValidade)
                          .ToList();
        }

        public async Task<List<Pet>> SearchPetsAsync(string searchTerm)
        {
            await Task.Delay(100);
            return _pets.Where(p => p.Nome.ToLower().Contains(searchTerm.ToLower()))
                       .OrderBy(p => p.Nome)
                       .ToList();
        }
    }
}