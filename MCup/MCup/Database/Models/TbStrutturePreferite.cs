using SQLite;

namespace MCup.Database.Models
{
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
