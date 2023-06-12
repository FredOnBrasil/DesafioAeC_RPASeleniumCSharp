using System;
using System.Windows;

namespace DesafioAeC_RPA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //configurando dados do banco de dados
        public static string folderPath = Environment.CurrentDirectory;

        public static string dataBasePObjectsName = "PageObjectsData.db";
        public static string dataBasePObjectsPath = System.IO.Path.Combine(folderPath, dataBasePObjectsName);

        public static string dataBasePesquisaName = "PesquisaData.db";
        public static string dataBasePesquisasPath = System.IO.Path.Combine(folderPath, dataBasePesquisaName);
    }
}
