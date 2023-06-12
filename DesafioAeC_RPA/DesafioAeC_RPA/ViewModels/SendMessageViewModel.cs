using MahApps.Metro.Controls.Dialogs;

namespace DesafioAeC_RPA.ViewModels
{
    public class SendMessage
    {
        private static MainWindow _mainWindow;

        public SendMessage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public static async void ShowMessageDialog(string mensagem, MahApps.Metro.Controls.MetroWindow window)
        {
            _mainWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Confirmar",
                AnimateShow = true,
                AnimateHide = true,
                ColorScheme = MetroDialogColorScheme.Accented
            };
            MessageDialogResult resultado = await DialogManager.ShowMessageAsync(window, "Desafio RPA AeC:", mensagem,
                MessageDialogStyle.Affirmative, mySettings);
            if (resultado == MessageDialogResult.Affirmative)
                return;
        }
    }
}
