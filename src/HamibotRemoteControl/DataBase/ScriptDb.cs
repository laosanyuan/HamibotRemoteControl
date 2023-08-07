using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;
using SQLite;
using System.Collections.ObjectModel;

namespace HamibotRemoteControl.DataBase
{
    internal class ScriptDb
    {
        private readonly SQLiteAsyncConnection _database;

        public ScriptDb()
        {
            var file = Path.Combine(AppPath.DataBaseFolder, "scripts.db");
            _database = new SQLiteAsyncConnection(file);
            _database.CreateTableAsync<Script>().Wait();
        }

        /// <summary>
        /// 获取脚本列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Script>> GetScripts()
        {
            return await _database.Table<Script>().ToListAsync();
        }

        /// <summary>
        /// 更新脚本列表
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        public async Task UpdateRobots(Collection<Script> scripts)
        {
            var existingRobots = await GetScripts();

            // 同步隐藏状态
            foreach (var script in scripts)
            {
                var existingRobot = existingRobots.FirstOrDefault(r => r.Id == script.Id);
                if (existingRobot != null)
                {
                    script.IsHidden = existingRobot.IsHidden;
                }
            }

            // 删除所有数据
            await _database.DeleteAllAsync<Script>();

            // 插入新数据
            await _database.InsertAllAsync(scripts);
        }
    }
}
