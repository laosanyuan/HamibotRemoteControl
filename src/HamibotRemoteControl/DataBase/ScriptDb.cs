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
        public async Task<List<Script>> GetAllScripts()
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
            var existingRobots = await GetAllScripts();

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

        /// <summary>
        /// 根据id获取脚本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Script> GetScript(string id)
        {
            return await _database.Table<Script>().FirstOrDefaultAsync(t => t.Id.Equals(id));
        }
    }
}
