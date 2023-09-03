using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Models.DataBase;

namespace HamibotRemoteControl.Models
{
    /// <summary>
    /// 快捷方案数据
    /// </summary>
    internal partial class ShortcutSchemeModel
    {
        /// <summary>
        /// id
        /// </summary>
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
        /// tag列表
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// 机器人id列表
        /// </summary>
        public List<string> RobotIds { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        public Script Script { get; set; }
        /// <summary>
        /// 机器人列表
        /// </summary>
        public List<Robot> Robots { get; set; }

        #region [Constructors]
        public static ShortcutSchemeModel CreateShortcutSchemeModelByRobot(
            string name,
            string scriptId,
            List<string> robotIds)
        {
            return new ShortcutSchemeModel()
            {
                SelectRobotType = SelectRobotType.Name,
                Name = name,
                ScriptId = scriptId,
                RobotIds = robotIds
            };
        }

        public static ShortcutSchemeModel CreateShortcutSchemeModelByTag(
            string name,
            string scriptId,
            List<string> tags)
        {
            return new ShortcutSchemeModel()
            {
                SelectRobotType = SelectRobotType.Tag,
                Name = name,
                ScriptId = scriptId,
                Tags = tags
            };
        }

        public ShortcutSchemeEntity ToShortcutSchemeEntity()
        {
            return new ShortcutSchemeEntity()
            {
                Id = Id,
                Name = Name,
                ScriptId = ScriptId,
                IncludeHiddenRobot = IncludeHiddenRobot,
                SelectRobotType = SelectRobotType,
                TagsStr = Tags is null ? null : string.Join("#", Tags),
                RobotIdsStr = RobotIds is null ? null : string.Join("#", RobotIds),
            };
        }
        #endregion
    }
}
