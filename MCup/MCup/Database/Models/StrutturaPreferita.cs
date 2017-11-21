using SQLite;

namespace MCup.Database.Models
{
    [Table("StrutturaPreferita")]
    public class StrutturaPreferita
    {

        [PrimaryKey, NotNull, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        public string strutturaPreferita { get; set; }
    }
}
