namespace DesafioAeC_RPA;

public class AppTearDown
{
    private MainWindow _mainWindow;

    public AppTearDown(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    internal void TearDown()
    {
        _mainWindow.driver.Quit();
    }
}