using System;
using DesafioAeC_RPA.Models;
using MahApps.Metro.Controls;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium.Support.UI;

namespace DesafioAeC_RPA
{
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Item> Items { get; set; }
        internal IWebDriver driver;
        internal IWebElement CampoPesquisa;
        internal IJavaScriptExecutor JavaScriptExecutor;

        public MainWindow()
        {
            InitializeComponent();

            Items = new ObservableCollection<Item>
            {
                new Item { Nome = "Action 1: Abre Site", ExecuteCommand = new RelayCommand(Abrir_Site) },
                new Item { Nome = "Action 2: Preenche Campo pesquisa", ExecuteCommand = new RelayCommand(PreencheCampoPesquisa) },
                new Item { Nome = "Action 4: Clica em Pesquisar", ExecuteCommand = new RelayCommand(ClickPesquisar) },
                new Item { Nome = "Action 5: Verifica resultados", ExecuteCommand = new RelayCommand(VerificaResultados) },
            };
            DataContext = this;
            _appSetup = new AppSetup(this);
            _appTearDown = new AppTearDown(this);
        }

        private void Abrir_Site()
        {
            try
            {
                if (BoxSiteSelection.SelectedIndex != -1)//indica seleção nula
                {
                    switch (BoxSiteSelection.SelectedItem.ToString())
                    {
                        case "Alura":
                            driver.Navigate().GoToUrl(App.UrlAlura);
                            resultados.Items.Add($"Ação executada: " + DateTime.Now);
                            break;

                        default:
                            driver.Navigate().GoToUrl(App.UrlDefault);
                            resultados.Items.Add($"Ação executada sem seleção de URL: " + DateTime.Now);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                resultados.Items.Add($"Erro ao executar ação: " + e.Message);
            }
        }

        private void PreencheCampoPesquisa()
        {
            CampoPesquisa = driver.FindElement(By.Id("header-barraBusca-form-campoBusca"));
            try
            {
                //if (CampoPesquisa.Displayed)
                //{
                    CampoPesquisa.Clear();
                    CampoPesquisa.SendKeys(TermoPesquisa.Text);
                    resultados.Items.Add($"Ação executada: " + DateTime.Now);
               // }
            }
            catch (Exception e)
            {
                resultados.Items.Add("Elemento não encontrado na tela.");
                resultados.Items.Add("Erro ao manipular elemento." + e.Message);
            }
        }

        private void ClickPesquisar()
        {
            //usando explicit wait para casos em que o elemento não é encontrado
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));

            try
            {
                var btPesquisa = wait.Until(d =>
                {
                    var botao = driver.FindElement(By.XPath("//button"));
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
                // Executar ação para o item atual
                await ExecuteCurrentItem();

                // Incrementar o índice para passar para o próximo item
                currentItemIndex++;

                // Chamar recursivamente para o próximo item após o término da execução atual
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

                //encerra o driver:
                _appTearDown.TearDown();
            }
        }

        private async Task ExecuteCurrentItem()
        {
            Item currentItem = Items[currentItemIndex];

            // Selecionar o item atual (opcional)
            ListView.SelectedItem = currentItem;

            // Rolar o ListView para o item atual
            ListView.ScrollIntoView(currentItem);

            await Task.Delay(300);

            resultados.Items.Add($"Executando o item: {currentItem.Nome}");

            // Lógica de execução para o item atual
            currentItem.ExecuteCommand.Execute(null);

            resultados.Items.Add($"Fim execução: {currentItem.Nome}");

            // Aguardar a finalização da execução do item atual
            await Task.Delay(600);
        }

        private void StartProcess_OnClick(object sender, RoutedEventArgs e)
        {
            resultados.Items.Clear();
            _appSetup.AppSetupInicialization();
            _ = ExecuteItemsLoop();
        }
    }
}
