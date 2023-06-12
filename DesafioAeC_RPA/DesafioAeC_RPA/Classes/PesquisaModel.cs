using SQLite;

namespace DesafioAeC_RPA.Classes
{
    [Table("PesquisaData")]
    public class PesquisaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Titulo { get; set; }
        [NotNull]
        public string CargaHoraria { get; set; }
        [NotNull]
        public string Professor { get; set; }
        [NotNull]
        public string Descrição { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Titulo} - {CargaHoraria} - {Professor} - {Descrição}";
        }
    }
}
