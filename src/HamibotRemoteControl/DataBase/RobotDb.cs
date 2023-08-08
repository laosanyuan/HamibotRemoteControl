using AutoMapper;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Models.DataBase;
using HamibotRemoteControl.Tools;
using SQLite;
using System.Collections.ObjectModel;

namespace HamibotRemoteControl.DataBase
{
    internal class RobotDb
    {
        private readonly IMapper _mapper;
        private readonly SQLiteAsyncConnection _database;

        public RobotDb()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Robot, RobotEntity>()
                    .ForMember(
                        dest => dest.TagStr,
                        opt => opt.MapFrom(src => string.Join("#", src.Tags)));
                cfg.CreateMap<RobotEntity, Robot>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.TagStr.Split('#', StringSplitOptions.None)));
            });
            _mapper = config.CreateMapper();

            var file = Path.Combine(AppPath.DataBaseFolder, "robots.db");
            _database = new SQLiteAsyncConnection(file);
            _database.CreateTableAsync<RobotEntity>().Wait();
        }

        /// <summary>
        /// 获取机器人列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Robot>> GetAllRobots()
        {
            var robotEntities = await _database.Table<RobotEntity>().ToListAsync();
            var robots = robotEntities.Select(_mapper.Map<Robot>).ToList();
            return robots;
        }

        /// <summary>
        /// 更新机器人列表
        /// </summary>
        /// <param name="robots"></param>
        /// <returns></returns>
        public async Task UpdateRobots(Collection<Robot> robots)
        {
            var existingRobots = await GetAllRobots();

            // 同步隐藏状态
            foreach (var newRobot in robots)
            {
                var existingRobot = existingRobots.FirstOrDefault(r => r.Id == newRobot.Id);
                if (existingRobot != null)
                {
                    newRobot.IsHidden = existingRobot.IsHidden;
                }
            }

            // 删除所有数据
            await _database.DeleteAllAsync<RobotEntity>();

            // 插入新数据
            await _database.InsertAllAsync(robots.Select(_mapper.Map<RobotEntity>));
        }

        /// <summary>
        /// 根据id获取机器人
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<Robot>> GetRobotByIds(List<string> ids, bool includeHidden = true)
        {
            var entities =
                await _database.Table<RobotEntity>()
                    .Where(t => includeHidden ? ids.Contains(t.Id) : ids.Contains(t.Id) && !t.IsHidden)
                    .ToListAsync();
            return entities.Select(_mapper.Map<Robot>).ToList();
        }

        /// <summary>
        /// 根据tag id 列表获取机器人
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<List<Robot>> GetRobotsByTags(List<string> tags, bool includeHidden = true)
        {
            var allRobots = await GetAllRobots();
            List<Robot> result = new();
            allRobots.ForEach(t =>
            {
                if (t.Tags.Intersect(tags).Any())
                {
                    result.Add(t);
                }
            });
            return result;
        }
    }
}
