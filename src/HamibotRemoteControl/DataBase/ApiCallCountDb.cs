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

        // 获取每日数据
        public async Task<List<DateCount>> GetCounts(DateTime? date = null)
        {
            await Init();
            List<ApiCallCount> datas;
            if (date == null)
            {
                datas = await _database.Table<ApiCallCount>().ToListAsync();
            }
            else
            {
                datas = await _database
                    .Table<ApiCallCount>()
                    .Where(t => t.Datetime >= date)
                    .ToListAsync();
            }

            var dailyCounts = datas
                .GroupBy(op => op.Datetime.Date)
                .Select(group => new DateCount
                {
                    Date = group.Key,
                    Count = group.Count()
                })
                .ToList();
            return dailyCounts;
        }

        /// <summary>
        /// 获取当月数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<DateCount>> GetCurrentMonthCounts()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return await GetCounts(date);
        }
    }

    public class DateCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
