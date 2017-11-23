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

        [NotNull, Column("DescrizioneStruttura")]
        public string DescrizioneStruttura { get; set; }

        public TbStrutturePreferite(int _id) { }

        public TbStrutturePreferite() { }
    }
}
