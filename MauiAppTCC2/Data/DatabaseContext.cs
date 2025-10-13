using MauiAppTCC2.Models;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MauiAppTCC2.Data
{
    public class DatabaseContext
    {
        private SQLiteAsyncConnection _database;

        public DatabaseContext()
        {
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // ✅ CRIAR TABELAS SE NÃO EXISTIREM
            await _database.CreateTableAsync<Pet>();
            await _database.CreateTableAsync<VacinaPet>();
        }

        // 📊 OPERAÇÕES PET
        public async Task<List<Pet>> GetPetsAsync()
        {
            return await _database.Table<Pet>()
                                .OrderBy(p => p.Nome)
                                .ToListAsync();
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            return await _database.Table<Pet>()
                                .Where(p => p.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<int> SavePetAsync(Pet pet)
        {
            if (pet.Id == 0)
            {
                // NOVO PET
                return await _database.InsertAsync(pet);
            }
            else
            {
                // ATUALIZAR PET EXISTENTE
                return await _database.UpdateAsync(pet);
            }
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            // ✅ PRIMEIRO EXCLUIR VACINAS DO PET
            var vacinas = await _database.Table<VacinaPet>()
                                       .Where(v => v.PetId == pet.Id)
                                       .ToListAsync();

            foreach (var vacina in vacinas)
            {
                await _database.DeleteAsync(vacina);
            }

            // ✅ DEPOIS EXCLUIR O PET
            return await _database.DeleteAsync(pet);
        }

        // 💉 OPERAÇÕES VACINA
        public async Task<List<VacinaPet>> GetVacinasByPetAsync(int petId)
        {
            return await _database.Table<VacinaPet>()
                                .Where(v => v.PetId == petId)
                                .OrderByDescending(v => v.DataAplicacao)
                                .ToListAsync();
        }

        public async Task<int> SaveVacinaAsync(VacinaPet vacina)
        {
            if (vacina.Id == 0)
            {
                return await _database.InsertAsync(vacina);
            }
            else
            {
                return await _database.UpdateAsync(vacina);
            }
        }

        public async Task<int> DeleteVacinaAsync(VacinaPet vacina)
        {
            return await _database.DeleteAsync(vacina);
        }

        public async Task<List<VacinaPet>> GetAllVacinasAsync()
        {
            return await _database.Table<VacinaPet>()
                                .OrderByDescending(v => v.DataAplicacao)
                                .ToListAsync();
        }

        public async Task<List<VacinaPet>> GetProximasVacinasVencerAsync()
        {
            var dataLimite = DateTime.Now.AddDays(30);
            return await _database.Table<VacinaPet>()
                                .Where(v => v.DataValidade <= dataLimite &&
                                           v.DataValidade >= DateTime.Now)
                                .OrderBy(v => v.DataValidade)
                                .ToListAsync();
        }

        public async Task<List<VacinaPet>> GetVacinasVencidasAsync()
        {
            return await _database.Table<VacinaPet>()
                                .Where(v => v.DataValidade < DateTime.Now)
                                .OrderBy(v => v.DataValidade)
                                .ToListAsync();
        }

        public async Task<List<Pet>> SearchPetsAsync(string searchTerm)
        {
            return await _database.Table<Pet>()
                                .Where(p => p.Nome.ToLower().Contains(searchTerm.ToLower()))
                                .OrderBy(p => p.Nome)
                                .ToListAsync();
        }

        // ✅ MÉTODO EXTRA: Contar total de pets
        public async Task<int> GetTotalPetsAsync()
        {
            return await _database.Table<Pet>().CountAsync();
        }

        // ✅ MÉTODO EXTRA: Contar total de vacinas
        public async Task<int> GetTotalVacinasAsync()
        {
            return await _database.Table<VacinaPet>().CountAsync();
        }
    }
}