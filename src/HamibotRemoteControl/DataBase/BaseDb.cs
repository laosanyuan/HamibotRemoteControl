using HamibotRemoteControl.Tools;
using SQLite;

namespace HamibotRemoteControl.DataBase
{
    internal abstract class BaseDb<T> where T : class, new()
    {
        protected SQLiteAsyncConnection _database;
        protected string _fileName;


        protected async Task Init()
        {
            if (_database == null)
            {
                var file = Path.Combine(AppPath.DataBaseFolder, this._fileName);
                _database = new SQLiteAsyncConnection(file, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                await _database.CreateTableAsync<T>();
            }
        }
    }
}
