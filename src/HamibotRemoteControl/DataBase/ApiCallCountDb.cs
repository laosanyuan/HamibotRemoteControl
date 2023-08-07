using HamibotRemoteControl.Models.DataBase;
using HamibotRemoteControl.Tools;
using SQLite;

namespace HamibotRemoteControl.DataBase
{
    /// <summary>
    /// API调用记录
    /// </summary>
    internal class ApiCallCountDb
    {
        private readonly SQLiteAsyncConnection _database;
        public ApiCallCountDb()
        {
            var file = Path.Combine(AppPath.DataBaseFolder, "api_call_count.db");
            _database = new SQLiteAsyncConnection(file);
            _database.CreateTableAsync<ApiCallCount>().Wait();
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task Insert(ApiCallCount count)
        {
            await _database.InsertAsync(count);
        }
    }
}
