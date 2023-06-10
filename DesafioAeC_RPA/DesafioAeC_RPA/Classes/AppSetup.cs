using System;
using OpenQA.Selenium;

namespace DesafioAeC_RPA;

public class AppSetup
{
    private MainWindow _mainWindow;

    public AppSetup(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public void AppSetupInicialization()
    {
        _mainWindow.driver = DesafioAeC_RPA.Models.WebActions.SelecionaBrowserMaximizaTela("Chrome");
        _mainWindow.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        _mainWindow.JavaScriptExecutor = (IJavaScriptExecutor)_mainWindow.driver;
    }
}