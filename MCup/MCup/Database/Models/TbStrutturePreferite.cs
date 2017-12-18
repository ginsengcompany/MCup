using SQLite;

namespace MCup.Database.Models
{
    //Classe che astrae la tabella TbStrutturePreferite nel database locale
    [Table("TbStrutturePreferite")]
    public class TbStrutturePreferite
    {
        [PrimaryKey, NotNull, Column("id")]
        public string id { get; set; }

        [NotNull, Column("NomeStruttura")]
        public string NomeStruttura { get; set; }

        public TbStrutturePreferite(string id, string nome)
        {
            this.id = id;
            this.NomeStruttura = nome;
        }

        public TbStrutturePreferite() { }
    }
}
