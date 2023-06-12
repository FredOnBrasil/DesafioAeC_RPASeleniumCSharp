using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesafioAeC_RPA.Classes;
using MahApps.Metro.Controls;

namespace DesafioAeC_RPA.ViewModels
{
    /// <summary>
    /// Interaction logic for PageObjectsViewModel.xaml
    /// </summary>
    public partial class PageObjectsViewModel : MetroWindow
    {
        private readonly DatabaseManager _dbManagerPageObjects = new DatabaseManager(App.dataBasePObjectsPath);
        private PageObjectsModel _pageObjectsModel;

        public PageObjectsViewModel()
        {
            InitializeComponent();

            // Criar tabela (se não existir)
            _dbManagerPageObjects.CreateTablesPageObjects();

            CarregaDadosTela();
        }

        private void CarregaDadosTela()
        {
            List<PageObjectsModel> pageObjects = _dbManagerPageObjects.GetAllPageObjects();
            ListViewPageObjects.ItemsSource = pageObjects;
        }

        private void InserirRegistro_OnClick(object sender, RoutedEventArgs e)
        {
            _dbManagerPageObjects.InsertPageObject(CapturaDadosTela());
            CarregaDadosTela();
        }

        private PageObjectsModel CapturaDadosTela()
        {
            _pageObjectsModel = new PageObjectsModel
            {
                Id = IdRegistro.Content != null ? (int)IdRegistro.Content : 1,
                Identificador = Identificador.Text,
                Nome = NomeElemento.Text,
                Tipo = cbxTipo.SelectedItem.ToString()
            };
            return _pageObjectsModel;
        }

        private void AtualizarRegistro_OnClick(object sender, RoutedEventArgs e)
        {
            _dbManagerPageObjects.UpdatePageObject(CapturaDadosTela());
            CarregaDadosTela();
        }

        private void ListViewPageObjects_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageObjectsModel selectedPageObject = (PageObjectsModel)ListViewPageObjects.SelectedItem;

            if (selectedPageObject != null)
            {
                var selectedIndex = cbxTipo.Items.Cast<object>()
                    .Select((item, index) => new { Item = item, Index = index })
                    .FirstOrDefault(x => x.Item.ToString() == selectedPageObject.Tipo)?.Index ?? -1;

                if (selectedIndex >= 0)
                {
                    cbxTipo.SelectedItem = selectedPageObject.Tipo;
                    cbxTipo.SelectedIndex = selectedIndex;
                }

                IdRegistro.Content = selectedPageObject.Id;
                Identificador.Text = selectedPageObject.Identificador;
                NomeElemento.Text = selectedPageObject.Nome;
            }
        }

        private void DeletarRegistro_OnClick(object sender, RoutedEventArgs e)
        {
            _dbManagerPageObjects.DeletePageObject(CapturaDadosTela());
            CarregaDadosTela();
        }
    }
}
