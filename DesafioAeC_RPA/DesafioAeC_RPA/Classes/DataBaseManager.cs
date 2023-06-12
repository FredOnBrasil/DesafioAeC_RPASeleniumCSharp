using SQLite;
using System.Collections.Generic;

namespace DesafioAeC_RPA.Classes
{
    public class DatabaseManager
    {
        private SQLiteConnection _connection;

        public DatabaseManager(string databasePath)
        {
            _connection = new SQLiteConnection(databasePath);
        }

        public void InsertPageObject(PageObjectsModel pageObjects)
        {
            _connection.Insert(pageObjects);
        }

        public void UpdatePageObject(PageObjectsModel pageObjects)
        {
            _connection.Update(pageObjects);
        }

        public void DeletePageObject(PageObjectsModel pageObjects)
        {
            _connection.Delete(pageObjects);
        }

        public void CreateTablesPageObjects()
        {
            _connection.CreateTable<PageObjectsModel>();
        }

        public List<PageObjectsModel> GetAllPageObjects()
        {
            return _connection.Table<PageObjectsModel>().ToList();
        }

        public void InsertPesquisa(PesquisaModel pesquisa)
        {
            _connection.Insert(pesquisa);
        }

        public void CreateTablesPesquisas()
        {
            _connection.CreateTable<PesquisaModel>();
        }

        public List<PesquisaModel> GetAllPesquisas()
        {
            return _connection.Table<PesquisaModel>().ToList();
        }
    }
}
