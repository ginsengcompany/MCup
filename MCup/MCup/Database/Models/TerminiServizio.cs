using SQLite;

namespace MCup.Database.Models
{
    //Classe che astrae la tabella TerminiServizio nel database locale
    [Table("TerminiServizio")]
    public class TerminiServizio
    {

        [PrimaryKey, NotNull, Column("accepted")]
        public bool accepted { get; set; }

        public TerminiServizio(bool acc)
        {
            accepted = acc;
        }

        public TerminiServizio()
        {
            accepted = false;
        }

    }
}
