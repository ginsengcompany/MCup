using MCup.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Database.Models
{
    //Classe che astrae la tabella TerminiServizio nel database locale
    [Table("Appuntamento")]
    public class Appuntamento
    {
        [PrimaryKey, AutoIncrement, Column("idPrenotazione")]
        private int idPrenotazione { get; set; }
        [NotNull, Column("data")]
        public string data { get; set; }
        [NotNull, Column("ora")]
        public string ora { get; set; }
        [NotNull,Column("nomeStruttura")]
        public string nomeStruttura { get; set; }
        [NotNull,Column("codiceStruttura")]
        public string codiceStruttura { get; set; }
        public Appuntamento() { }

        public Appuntamento(Prenotazione prenotazione)
        {
            this.data = prenotazione.data;
            this.ora = prenotazione.ora;
            this.codiceStruttura = prenotazione.codiceStruttura;
            this.nomeStruttura = prenotazione.nomeStruttura;
        }
    }
}
