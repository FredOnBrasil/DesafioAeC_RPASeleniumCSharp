using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DesafioAeC_RPA.Models
{
    public class WebActions
    {
        #region Seleção Browser
        public static IWebDriver SelecionaBrowserMaximizaTela(string browser)
        {
            IWebDriver driver;
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver();
                    driver.Manage().Window.Maximize();
                    break;

                default:
                    driver = new ChromeDriver();
                    driver.Manage().Window.Maximize();
                    break;
            }
            return driver;
        }
        #endregion


    }
}
