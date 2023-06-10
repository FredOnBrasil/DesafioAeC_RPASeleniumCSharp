using System;
using DesafioAeC_RPA.Models;
using MahApps.Metro.Controls;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace DesafioAeC_RPA
{
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Item> Items { get; set; }
        internal IWebDriver driver;
        internal IJavaScriptExecutor js;

        public MainWindow()
        {
            InitializeComponent();

            Items = new ObservableCollection<Item>
            {
                new Item { Nome = "Action 1: Abre Site", ExecuteCommand = new RelayCommand(Abrir_Site) },
                new Item { Nome = "Action 2: Seleciona Campo pesquisa", ExecuteCommand = new RelayCommand(SelecionaCampoPesquisa) },
                new Item { Nome = "Action 3: Executa a pesquisa e obtém os resultados", ExecuteCommand = new RelayCommand(ExecuteItem3) },
                new Item { Nome = "Action 4: Abre Site", ExecuteCommand = new RelayCommand(Abrir_Site) },
                new Item { Nome = "Action 5: Seleciona Campo pesquisa", ExecuteCommand = new RelayCommand(SelecionaCampoPesquisa) },
                new Item { Nome = "Action 6: Executa a pesquisa e obtém os resultados", ExecuteCommand = new RelayCommand(ExecuteItem3) }
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
                            break;

                        default:
                            driver.Navigate().GoToUrl(App.UrlDefault);
                            break;
                    }
                    ListView.Items.Add($"Ação executada: {this.Name} _" + DateTime.Now.Date);
                }
                else
                {
                    driver.Navigate().GoToUrl(App.UrlDefault);
                    resultados.Items.Add($"Ação executada sem seleção de URL: {this.Name} _" + DateTime.Now.Date);
                }
            }
            catch (Exception e)
            {
                resultados.Items.Add($"Erro ao executar ação: {this.Name} - " + e.Message);
            }
        }

        private void SelecionaCampoPesquisa()
        {
            // Executar a lógica para o Item 2
        }

        private void ExecuteItem3()
        {
            // Executar a lógica para o Item 3
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

            // Rolar o ListView para o item atual
            ListView.ScrollIntoView(currentItem);

            resultados.Items.Add($"Executando o item: {currentItem.Nome}");

            // Lógica de execução para o item atual
            currentItem.ExecuteCommand.Execute(null);

            // Selecionar o item atual (opcional)
            ListView.SelectedItem = currentItem;

            // Aguardar a finalização da execução do item atual
            await Task.Delay(1000); // Exemplo: espera de 1 segundo
        }

        private void StartProcess_OnClick(object sender, RoutedEventArgs e)
        {
            _appSetup.AppSetupInicialization();
            ExecuteItemsLoop();
        }
    }
}
