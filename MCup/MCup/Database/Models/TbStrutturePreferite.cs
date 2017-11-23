using SQLite;

namespace MCup.Database.Models
{
    [Table("TbStrutturePreferite")]
    public class TbStrutturePreferite
    {
        [PrimaryKey, NotNull, AutoIncrement, Column("id")]
        public int id { get; set; }

        [NotNull]
        public string NomeStruttura { get; set; }

        [NotNull]
        public string DescrizioneStruttura { get; set; }

        public TbStrutturePreferite(int _id) { }

        public TbStrutturePreferite() { }
    }
}
