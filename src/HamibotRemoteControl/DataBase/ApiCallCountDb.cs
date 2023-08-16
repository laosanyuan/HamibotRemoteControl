using HamibotRemoteControl.Models.DataBase;

namespace HamibotRemoteControl.DataBase
{
    /// <summary>
    /// API调用记录
    /// </summary>
    internal class ApiCallCountDb : BaseDb<ApiCallCount>
    {
        public ApiCallCountDb()
        {
            base._fileName = "api_call_count.db";
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task Insert(ApiCallCount count)
        {
            await Init();
            await _database.InsertAsync(count);
        }
    }
}
