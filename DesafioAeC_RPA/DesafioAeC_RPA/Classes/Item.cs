using System.Windows.Input;

namespace DesafioAeC_RPA.Models
{
    public class Item
    {
        public string Nome { get; set; }
        public ICommand ExecuteCommand { get; set; }
    }

}
