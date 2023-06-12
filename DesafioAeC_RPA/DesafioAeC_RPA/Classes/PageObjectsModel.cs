using SQLite;

namespace DesafioAeC_RPA.Classes
{
    [Table("PageObjectsData")]
    public class PageObjectsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Nome { get; set; }
        [NotNull]
        public string Identificador { get; set; }
        [NotNull]
        public string Tipo { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Nome} - {Identificador} - {Tipo}";
        }
    }
}
