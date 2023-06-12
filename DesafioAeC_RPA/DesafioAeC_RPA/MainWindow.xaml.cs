using System;
using System.Collections.Generic;
using DesafioAeC_RPA.Models;
using MahApps.Metro.Controls;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DesafioAeC_RPA.ViewModels;
using OpenQA.Selenium.Support.UI;
using DesafioAeC_RPA.Classes;

namespace DesafioAeC_RPA
{
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Item> Items { get; set; }
        internal IWebDriver driver;
        internal IJavaScriptExecutor JavaScriptExecutor;
        private readonly DatabaseManager _dbManagerPageObjects = new DatabaseManager(App.dataBasePObjectsPath);
        private List<PageObjectsModel> identificadores = new List<PageObjectsModel>();

        public MainWindow()
        {
            InitializeComponent();

            MetroDialog = new SendMessage(this);

            BoxSiteSelection.SelectedIndex = 0;

            // Criar tabela (se não existir)
            _dbManagerPageObjects.CreateTablesPageObjects();

            CarregaDadosIdentificadores();

            Items = new ObservableCollection<Item>
            {
                new Item { Nome = "Action 1: Abre Site", ExecuteCommand = new RelayCommand(Abrir_Site) },
                new Item { Nome = "Action 2: Preenche Campo pesquisa", ExecuteCommand = new RelayCommand(PreencheCampoPorId) },
                new Item { Nome = "Action 3: Clica em Pesquisar", ExecuteCommand = new RelayCommand(ClickPorXpath) },
                new Item { Nome = "Action 6: Verifica resultados", ExecuteCommand = new RelayCommand(VerificaResultados) },
            };
            DataContext = this;

            _appSetup = new AppSetup(this);
            _appTearDown = new AppTearDown(this);
        }


        internal SendMessage MetroDialog { get; set; }
        private string url = "";
        private string nomeSite = "";
        private string CampoPesquisaId = "";
        private string xpathPesquisar = "";

        private void CarregaDadosIdentificadores()
        {
            identificadores = _dbManagerPageObjects.GetAllPageObjects();

            nomeSite = BoxSiteSelection.SelectedItem.ToString()!.ToLower();

            foreach (var item in identificadores)
            {
                if (item.Tipo == "Url" && item.Nome.ToLower().Contains(nomeSite))
                    url = item.Identificador;

                if (item.Tipo == "Id" && item.Nome.ToLower().Contains(nomeSite))
                    CampoPesquisaId = item.Identificador;

                if (item.Tipo == "Xpath" && item.Nome.ToLower().Contains(nomeSite))
                    xpathPesquisar = item.Identificador;
            }
        }

        private void Abrir_Site()
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                resultados.Items.Add($"Ação executada: " + DateTime.Now);
            }
            catch (Exception e)
            {
                resultados.Items.Add($"Erro ao executar ação: " + e.Message);
            }

        }

        private void PreencheCampoPorId()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));

            try
            {
                var campoSelecionado = wait.Until(d =>
                {
                    var selecao = driver.FindElement(By.Id(CampoPesquisaId));
                    if (selecao.Displayed)
                        return selecao;
                    return null;
                });
                campoSelecionado.Clear();
                campoSelecionado.SendKeys(TermoPesquisa.Text);
                resultados.Items.Add($"Ação executada: " + DateTime.Now);
            }
            catch (Exception e)
            {
                resultados.Items.Add("Elemento não encontrado na tela.");
                resultados.Items.Add("Erro ao manipular elemento." + e.Message);
            }
        }

        private void ClickPorXpath()
        {
            //usando explicit wait para casos em que o elemento não é encontrado
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            try
            {
                var btPesquisa = wait.Until(d =>
                {
                    var botao = driver.FindElement(By.XPath(xpathPesquisar));
                    if (botao.Displayed)
                        return botao;
                    return null;
                });
                btPesquisa.Click();
                resultados.Items.Add($"Ação executada: " + DateTime.Now);

            }
            catch (Exception e)
            {
                resultados.Items.Add("Elemento não encontrado na tela.");
                resultados.Items.Add("Erro ao clicar em elemento." + e.Message);
            }
        }

        private void VerificaResultados()
        {
            // Executar a lógica para o Item 3
        }

        private void ExecutarJavascript(string script, IWebElement elemento)
        {
            JavaScriptExecutor = (IJavaScriptExecutor)driver;
            script = script.Replace("\\", "");

            JavaScriptExecutor.ExecuteScript(script, elemento);

            //Thread.Sleep(6000);
        }

        private int currentItemIndex = 0;
        private readonly AppSetup _appSetup;
        private readonly AppTearDown _appTearDown;

        private async Task ExecuteItemsLoop()
        {

            if (currentItemIndex < Items.Count)
            {
                await ExecuteCurrentItem();

                currentItemIndex++;

                await ExecuteItemsLoop();
            }
            else
            {
                currentItemIndex = 0;
                if (Items.Count > 0)
                {
                    ListView.SelectedItem = Items[0];
                }

                //scroll ate o primeiro item da lista
                ListView.ScrollIntoView(Items[0]);
                ListView.SelectedItem = null;

                _appTearDown.TearDown();
            }
        }

        private async Task ExecuteCurrentItem()
        {
            Item currentItem = Items[currentItemIndex];

            ListView.SelectedItem = currentItem;

            ListView.ScrollIntoView(currentItem);

            await Task.Delay(300);

            resultados.Items.Add($"Executando o item: {currentItem.Nome}");

            currentItem.ExecuteCommand.Execute(null);

            resultados.Items.Add($"Fim execução: {currentItem.Nome}");

            await Task.Delay(600);
        }

        private void StartProcess_OnClick(object sender, RoutedEventArgs e)
        {
            CarregaDadosIdentificadores();
            resultados.Items.Clear();

            if (!string.IsNullOrEmpty(TermoPesquisa.Text))
            {
                _appSetup.AppSetupInicialization();
                _ = ExecuteItemsLoop();
            }
            else
            {
                SendMessage.ShowMessageDialog("Preencha o campo com o texto a ser pesquisado", this);
            }
        }

        private void PageObjects_OnClick(object sender, RoutedEventArgs e)
        {
            PageObjectsViewModel pageObjectsView = new PageObjectsViewModel();
            pageObjectsView.ShowDialog();
        }
    }
}
