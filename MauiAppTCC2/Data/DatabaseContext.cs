using MauiAppTCC.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using MauiAppTCC2.Data;
using MauiAppTCC2.Models;

namespace MauiAppTCC.Data
{
    public class DatabaseContext
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized = false;

        public object SQLiteOpenFlags { get; private set; }

        public DatabaseContext()
        {
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            if (_initialized) return;

            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "petvaccine.db3");

                _database = new SQLiteAsyncConnection(databasePath,
                    SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);

                // Criar tabelas
                await _database.CreateTableAsync<Pet>();
                await _database.CreateTableAsync<Vacina>();

                _initialized = true;

                Debug.WriteLine("✅ Banco de dados inicializado com sucesso!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erro ao inicializar banco: {ex.Message}");
                throw;
            }
        }

        // 📊 OPERAÇÕES PET
        public async Task<List<Pet>> GetPetsAsync()
        {
            await InitializeAsync();
            return await _database.Table<Pet>().OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<Pet>().Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SavePetAsync(Pet pet)
        {
            await InitializeAsync();

            if (pet.Id == 0)
                return await _database.InsertAsync(pet);
            else
                return await _database.UpdateAsync(pet);
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            await InitializeAsync();

            // Primeiro deleta todas as vacinas do pet
            var vacinas = await GetVacinasByPetAsync(pet.Id);
            foreach (var vacina in vacinas)
            {
                await _database.DeleteAsync(vacina);
            }

            return await _database.DeleteAsync(pet);
        }

        // 💉 OPERAÇÕES VACINA
        public async Task<List<Vacina>> GetVacinasByPetAsync(int petId)
        {
            await InitializeAsync();
            return await _database.Table<Vacina>()
                                .Where(v => v.PetId == petId)
                                .OrderByDescending(v => v.DataAplicacao)
                                .ToListAsync();
        }

        public async Task<List<Vacina>> GetProximasVacinasVencerAsync()
        {
            await InitializeAsync();
            var dataLimite = DateTime.Now.AddDays(30);
            return await _database.Table<Vacina>()
                                .Where(v => v.DataValidade <= dataLimite &&
                                           v.DataValidade >= DateTime.Now)
                                .OrderBy(v => v.DataValidade)
                                .ToListAsync();
        }

        public async Task<int> SaveVacinaAsync(Vacina vacina)
        {
            await InitializeAsync();

            if (vacina.Id == 0)
                return await _database.InsertAsync(vacina);
            else
                return await _database.UpdateAsync(vacina);
        }

        public async Task<int> DeleteVacinaAsync(Vacina vacina)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(vacina);
        }

        public async Task<List<Vacina>> GetAllVacinasAsync()
        {
            await InitializeAsync();
            return await _database.Table<Vacina>().ToListAsync();
        }
    }
}