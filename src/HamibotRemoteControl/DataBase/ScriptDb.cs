using HamibotRemoteControl.Models;

namespace HamibotRemoteControl.DataBase
{
    internal class ScriptDb : BaseDb<Script>
    {
        public ScriptDb()
        {
            base._fileName = "scripts.db";
        }

        /// <summary>
        /// 获取脚本列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Script>> GetAllScripts()
        {
            await base.Init();
            return await _database.Table<Script>().ToListAsync();
        }

        /// <summary>
        /// 更新脚本列表
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        public async Task UpdateRobots(IList<Script> scripts)
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
            try
            {
                await Init();
                return await _database.Table<Script>().FirstOrDefaultAsync(t => t.Id.Equals(id));
            }
            catch (Exception ex)
            {
                // log
                return null;
            }
        }
    }
}
