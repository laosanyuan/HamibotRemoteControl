using SQLite;

namespace HamibotRemoteControl.Models.DataBase
{
    public class RobotEntity
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string TagStr { get; set; }

        /// <summary>
        /// 手机品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 手机型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Hamibot版本号
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// 是否被用户隐藏
        /// </summary>
        public bool IsHidden { get; set; }

        public Robot ToRobot()
        {
            return new Robot
            {
                Id = Id,
                Name = Name,
                Online = Online,
                Tags = string.IsNullOrEmpty(TagStr) ? null : TagStr?.Split("#")?.ToList(),
                Brand = Brand,
                Model = Model,
                AppVersion = AppVersion,
                IsHidden = IsHidden
            };
        }
    }
}
