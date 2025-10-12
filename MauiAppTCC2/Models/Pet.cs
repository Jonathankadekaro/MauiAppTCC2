using SQLite;

namespace MauiAppTCC2.Models
{
    public class Pet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(50)]
        public string Especie { get; set; } // Cachorro, Gato, etc.

        [MaxLength(50)]
        public string Raca { get; set; }

        public DateTime DataNascimento { get; set; }
        public double Peso { get; set; }

        [MaxLength(500)]
        public string FotoPath { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Ignore]
        public int Idade => CalcularIdade();

        private int CalcularIdade()
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;

            // Ajusta se ainda não fez aniversário este ano
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }

        // Método útil para exibição (opcional)
        [Ignore]
        public string IdadeFormatada
        {
            get
            {
                if (Idade <= 0) return "Recém-nascido";
                return $"{Idade} ano{(Idade != 1 ? "s" : "")}";
            }
        }
    }
}