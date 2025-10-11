
using SQLite;
namespace MauiAppTCC2.Models

{
    public record Pet([property: PrimaryKey, AutoIncrement] int Id, [property: SQLite.MaxLength(100)] string Nome, [property: SQLite.MaxLength(50)] string Especie, [property: SQLite.MaxLength(50)] string Raca, DateTime DataNascimento, double Peso, [property: SQLite.MaxLength(500)] string FotoPath)
    {
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Ignore]
        public int Idade => CalculateAge();

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DataNascimento.Year;
            if (DataNascimento.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}