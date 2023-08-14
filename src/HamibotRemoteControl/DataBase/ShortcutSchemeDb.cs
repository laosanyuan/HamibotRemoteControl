using AutoMapper;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Models.DataBase;
using HamibotRemoteControl.Tools;
using SQLite;

namespace HamibotRemoteControl.DataBase
{
    /// <summary>
    /// 管理快捷方案
    /// </summary>
    internal class ShortcutSchemeDb
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly IMapper _mapper;

        public ShortcutSchemeDb()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShortcutSchemeModel, ShortcutSchemeEntity>()
                    .ForMember(
                        dest => dest.TagsStr,
                        opt => opt.MapFrom(src => string.Join("#", src.Tags)))
                    .ForMember(
                        dest => dest.RobotIdsStr,
                        opt => opt.MapFrom(src => string.Join("#", src.RobotIds)));
                cfg.CreateMap<ShortcutSchemeEntity, ShortcutSchemeModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.TagsStr.Split('#', StringSplitOptions.None)))
                    .ForMember(
                        dest => dest.RobotIds,
                        opt => opt.MapFrom(src => src.RobotIdsStr.Split('#', StringSplitOptions.None)));
            });
            _mapper = config.CreateMapper();

            var file = Path.Combine(AppPath.DataBaseFolder, "shortcut_scheme.db");
            _database = new SQLiteAsyncConnection(file);
            _database.CreateTableAsync<ShortcutSchemeEntity>().Wait();
        }

        /// <summary>
        /// 获取所有方案
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShortcutSchemeModel>> GetAllSchemes()
        {
            var tmp = await _database.Table<ShortcutSchemeEntity>().ToListAsync();
            var schemes = tmp.Select(_mapper.Map<ShortcutSchemeModel>).ToList();
            return schemes;
        }

        /// <summary>
        /// 根据id获取快捷方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShortcutSchemeModel> GetScheme(string id)
        {
            var tmp = await _database.Table<ShortcutSchemeEntity>().FirstOrDefaultAsync(t => t.Id.Equals(id));
            return _mapper.Map<ShortcutSchemeModel>(tmp);
        }

        /// <summary>
        /// 更新快捷方案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateScheme(ShortcutSchemeModel model)
        {
            var tmp = _mapper.Map<ShortcutSchemeEntity>(model);
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
            var tmp = _mapper.Map<ShortcutSchemeEntity>(model);
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
            return await _database.DeleteAsync<ShortcutSchemeEntity>(id) > 0;
        }
    }
}
