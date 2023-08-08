using HamibotRemoteControl.Enums;
using SQLite;

namespace HamibotRemoteControl.Models.DataBase
{
    internal class ShortcutSchemeEntity
    {
        /// <summary>
        /// 方案名称
        /// </summary>
        [PrimaryKey]
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
    }
}
