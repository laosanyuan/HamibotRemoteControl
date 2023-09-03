using HamibotRemoteControl.Models;
using HamibotRemoteControl.Models.DataBase;
using System.Collections.ObjectModel;

namespace HamibotRemoteControl.DataBase
{
    internal class RobotDb : BaseDb<RobotEntity>
    {
        public RobotDb()
        {
            base._fileName = "robots.db";
        }

        /// <summary>
        /// 获取机器人列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Robot>> GetAllRobots()
        {
            await Init();
            var robotEntities = await _database.Table<RobotEntity>().ToListAsync();
            var robots = robotEntities.Select(t => t.ToRobot()).ToList();
            return robots;
        }

        /// <summary>
        /// 更新机器人列表
        /// </summary>
        /// <param name="robots"></param>
        /// <returns></returns>
        public async Task UpdateRobots(IList<Robot> robots)
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
            await _database.InsertAllAsync(robots.Select(t => t.ToRobotEntity()));
        }

        /// <summary>
        /// 根据id获取机器人
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<Robot>> GetRobotByIds(List<string> ids, bool includeHidden = true)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Robot>();
            }
            await Init();
            var query = _database.Table<RobotEntity>().Where(t => ids.Contains(t.Id));

            if (!includeHidden)
            {
                query = query.Where(t => !t.IsHidden);
            }

            var entities = await query.ToListAsync();
            return entities.Select(t => t.ToRobot()).ToList();
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
                if (tags != null
                    && t?.Tags != null
                    && t.Tags.Intersect(tags).Any())
                {
                    result.Add(t);
                }
            });
            return result;
        }

        /// <summary>
        /// 获取所有tag
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllTags()
        {
            await Init();

            List<string> tags = new();
            foreach (var robot in await _database.Table<RobotEntity>().ToListAsync())
            {
                if (!string.IsNullOrEmpty(robot.TagStr))
                {
                    var tmps = robot.TagStr.Split('#', StringSplitOptions.None);
                    foreach (var tmp in tmps)
                    {
                        if (!tags.Contains(tmp))
                        {
                            tags.Add(tmp);
                        }
                    }
                }
            }

            return tags;
        }
    }
}
