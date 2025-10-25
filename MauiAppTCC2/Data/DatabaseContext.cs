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
            await _database.CreateTableAsync<Usuario>();
            await _database.CreateTableAsync<Pet>();
            await _database.CreateTableAsync<VacinaPet>();
        }

        // 👤 OPERAÇÕES USUÁRIO
        public async Task<List<Usuario>> GetAllUsuariosAsync()
        {
            return await _database.Table<Usuario>().ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _database.Table<Usuario>()
                                .Where(u => u.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _database.Table<Usuario>()
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveUsuarioAsync(Usuario usuario)
        {
            if (usuario.Id == 0)
            {
                return await _database.InsertAsync(usuario);
            }
            else
            {
                return await _database.UpdateAsync(usuario);
            }
        }

        // 📊 OPERAÇÕES PET
        public async Task<List<Pet>> GetPetsAsync()
        {
            return await _database.Table<Pet>()
                                .OrderBy(p => p.Nome)
                                .ToListAsync();
        }

        public async Task<List<Pet>> GetPetsByUsuarioAsync(int usuarioId)
        {
            return await _database.Table<Pet>()
                                .Where(p => p.UsuarioId == usuarioId)
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

        public async Task<List<VacinaPet>> GetVacinasByUsuarioAsync(int usuarioId)
        {
            // ✅ CORREÇÃO: SEM JOIN - BUSCA EM DUAS ETAPAS
            var petsDoUsuario = await GetPetsByUsuarioAsync(usuarioId);
            var petIds = petsDoUsuario.Select(p => p.Id).ToList();

            if (!petIds.Any())
                return new List<VacinaPet>();

            return await _database.Table<VacinaPet>()
                                .Where(v => petIds.Contains(v.PetId))
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

        public async Task<List<VacinaPet>> GetProximasVacinasVencerByUsuarioAsync(int usuarioId)
        {
            // ✅ CORREÇÃO: SEM JOIN
            var petsDoUsuario = await GetPetsByUsuarioAsync(usuarioId);
            var petIds = petsDoUsuario.Select(p => p.Id).ToList();

            if (!petIds.Any())
                return new List<VacinaPet>();

            var dataLimite = DateTime.Now.AddDays(30);
            return await _database.Table<VacinaPet>()
                                .Where(v => petIds.Contains(v.PetId) &&
                                           v.DataValidade <= dataLimite &&
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

        public async Task<List<VacinaPet>> GetVacinasVencidasByUsuarioAsync(int usuarioId)
        {
            // ✅ CORREÇÃO: SEM JOIN
            var petsDoUsuario = await GetPetsByUsuarioAsync(usuarioId);
            var petIds = petsDoUsuario.Select(p => p.Id).ToList();

            if (!petIds.Any())
                return new List<VacinaPet>();

            return await _database.Table<VacinaPet>()
                                .Where(v => petIds.Contains(v.PetId) &&
                                           v.DataValidade < DateTime.Now)
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

        public async Task<List<Pet>> SearchPetsByUsuarioAsync(int usuarioId, string searchTerm)
        {
            return await _database.Table<Pet>()
                                .Where(p => p.UsuarioId == usuarioId &&
                                           p.Nome.ToLower().Contains(searchTerm.ToLower()))
                                .OrderBy(p => p.Nome)
                                .ToListAsync();
        }

        // ✅ MÉTODOS EXTRA
        public async Task<int> GetTotalPetsAsync()
        {
            return await _database.Table<Pet>().CountAsync();
        }

        public async Task<int> GetTotalPetsByUsuarioAsync(int usuarioId)
        {
            return await _database.Table<Pet>()
                                .Where(p => p.UsuarioId == usuarioId)
                                .CountAsync();
        }

        public async Task<int> GetTotalVacinasAsync()
        {
            return await _database.Table<VacinaPet>().CountAsync();
        }

        public async Task<int> GetTotalVacinasByUsuarioAsync(int usuarioId)
        {
            // ✅ CORREÇÃO: SEM JOIN
            var petsDoUsuario = await GetPetsByUsuarioAsync(usuarioId);
            var petIds = petsDoUsuario.Select(p => p.Id).ToList();

            if (!petIds.Any())
                return 0;

            return await _database.Table<VacinaPet>()
                                .Where(v => petIds.Contains(v.PetId))
                                .CountAsync();
        }
    }
}