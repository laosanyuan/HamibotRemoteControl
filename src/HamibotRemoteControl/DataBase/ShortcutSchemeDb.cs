using HamibotRemoteControl.Models;
using HamibotRemoteControl.Models.DataBase;

namespace HamibotRemoteControl.DataBase
{
    /// <summary>
    /// 管理快捷方案
    /// </summary>
    internal class ShortcutSchemeDb : BaseDb<ShortcutSchemeEntity>
    {
        public ShortcutSchemeDb()
        {
            base._fileName = "shortcut_scheme.db";
        }

        /// <summary>
        /// 获取所有方案
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShortcutSchemeModel>> GetAllSchemes()
        {
            await Init();
            var tmp = await _database.Table<ShortcutSchemeEntity>().ToListAsync();
            var schemes = tmp.Select(t => t.ToShortSchemeModel()).ToList();
            return schemes;
        }

        /// <summary>
        /// 根据id获取快捷方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShortcutSchemeModel> GetScheme(string id)
        {
            await Init();
            var tmp = await _database.Table<ShortcutSchemeEntity>().FirstOrDefaultAsync(t => t.Id.Equals(id));
            return tmp.ToShortSchemeModel();
        }

        /// <summary>
        /// 更新快捷方案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateScheme(ShortcutSchemeModel model)
        {
            await Init();
            var tmp = model.ToShortcutSchemeEntity();
            var count = await _database.UpdateAsync(tmp);
            return count > 0;
        }

        /// <summary>
        /// 插入新方案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertScheme(ShortcutSchemeModel model)
        {
            await Init();
            var tmp = model.ToShortcutSchemeEntity();
            var count = await _database.InsertAsync(tmp);
            return count > 0;
        }

        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteScheme(string id)
        {
            await Init();
            return await _database.DeleteAsync<ShortcutSchemeEntity>(id) > 0;
        }
    }
}
