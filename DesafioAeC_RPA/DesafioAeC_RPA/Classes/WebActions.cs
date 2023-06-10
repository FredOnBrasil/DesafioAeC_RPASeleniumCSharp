using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
