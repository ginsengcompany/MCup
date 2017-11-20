using SQLite;

namespace MCup.Database.Models
{
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
