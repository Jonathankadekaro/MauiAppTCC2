using SQLite;

namespace MauiAppTCC2.Models
{
    public class VacinaPet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int PetId { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(100)]
        public string Fabricante { get; set; }

        [MaxLength(50)]
        public string Lote { get; set; }

        public DateTime DataAplicacao { get; set; }
        public DateTime DataValidade { get; set; }

        [MaxLength(50)]
        public string Dose { get; set; }

        [MaxLength(100)]
        public string VeterinarioResponsavel { get; set; }

        [MaxLength(100)]
        public string Clinica { get; set; }

        public bool NotificacaoAgendada { get; set; }

        [Ignore]
        public bool EstaVencida => DataValidade < DateTime.Now;

        [Ignore]
        public bool VenceEm30Dias => DataValidade <= DateTime.Now.AddDays(30) && DataValidade > DateTime.Now;

        // ✅ PROPRIEDADES PARA AS CORES E TEXTO DO STATUS
        [Ignore]
        public string StatusColor => EstaVencida ? "#FF4444" : "#4CAF50";

        [Ignore]
        public string StatusTexto => EstaVencida ? "VENCIDA" : "EM DIA";
    }
}