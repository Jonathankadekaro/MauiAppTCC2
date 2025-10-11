using Plugin.LocalNotification;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTCC2.Models
{
    public class Vacinapet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int PetId { get; set; } // Chave estrangeira

        [MaxLength(100)]
        public string Nome { get; set; } // V8, V10, Antirrábica, etc.

        [MaxLength(100)]
        public string Fabricante { get; set; }

        [MaxLength(50)]
        public string Lote { get; set; }

        public DateTime DataAplicacao { get; set; }
        public DateTime DataValidade { get; set; }

        [MaxLength(50)]
        public string Dose { get; set; } // 1ª dose, Reforço anual

        [MaxLength(100)]
        public string VeterinarioResponsavel { get; set; }

        [MaxLength(100)]
        public string Clinica { get; set; }

        public bool NotificacaoAgendada { get; set; }

        [Ignore]
        public bool EstaVencida => DataValidade < DateTime.Now;

        [Ignore]
        public bool VenceEm30Dias => DataValidade <= DateTime.Now.AddDays(30) &&
                                   DataValidade > DateTime.Now;
    }
}