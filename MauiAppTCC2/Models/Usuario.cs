using SQLite;

namespace MauiAppTCC2.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Senha { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}