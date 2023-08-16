using HamibotRemoteControl.Enums;
using SQLite;

namespace HamibotRemoteControl.Models.DataBase
{
    internal class ShortcutSchemeEntity
    {
        /// <summary>
        /// id
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取机器人方式
        /// </summary>
        public SelectRobotType SelectRobotType { get; set; }
        /// <summary>
        /// 筛选时是否包含已被隐藏机器人
        /// </summary>
        public bool IncludeHiddenRobot { get; set; } = true;
        /// <summary>
        /// 脚本id
        /// </summary>
        public string ScriptId { get; set; }
        /// <summary>
        /// Tag列表，根据SelectRobotType.Tag
        /// </summary>
        public string TagsStr { get; set; }
        /// <summary>
        /// 机器人id列表，根据SelectRobotType.Name
        /// </summary>
        public string RobotIdsStr { get; set; }

        public ShortcutSchemeModel ToShortSchemeModel()
        {
            return new ShortcutSchemeModel
            {
                Id = Id,
                Name = Name,
                SelectRobotType = SelectRobotType,
                IncludeHiddenRobot = IncludeHiddenRobot,
                ScriptId = ScriptId,
                Tags = string.IsNullOrEmpty(TagsStr) ? null : TagsStr?.Split('#').ToList(),
                RobotIds = string.IsNullOrEmpty(RobotIdsStr) ? null : RobotIdsStr?.Split('#').ToList()
            };
        }
    }
}
