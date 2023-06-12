using DesafioAeC_RPA.Classes;
using MahApps.Metro.Controls;
using System.Collections.Generic;

namespace DesafioAeC_RPA.ViewModels
{
    /// <summary>
    /// Interaction logic for PesquisasViewModel.xaml
    /// </summary>
    public partial class PesquisasViewModel : MetroWindow
    {
        private readonly DatabaseManager _dbManagerPesquisa = new DatabaseManager(App.dataBasePesquisasPath);

        public PesquisasViewModel()
        {
            InitializeComponent();

            _dbManagerPesquisa.CreateTablesPesquisas();
            CarregaDadosTela();
        }

        private void CarregaDadosTela()
        {
            List<PesquisaModel> pesquisas = _dbManagerPesquisa.GetAllPesquisas();
            ListViewPesquisas.ItemsSource = pesquisas;
        }
    }
}
