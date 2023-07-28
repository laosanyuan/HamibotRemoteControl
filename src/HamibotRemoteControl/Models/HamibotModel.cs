using System.Text.Json.Serialization;

namespace HamibotRemoteControl.Models
{
    #region [Scripts]
    /// <summary>
    /// 脚本集合
    /// </summary>
    public class ScriptCollection
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("items")]
        public List<Script> Items { get; set; }
    }

    /// <summary>
    /// 脚本基本信息
    /// </summary>
    public class BaseScript
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 脚本信息
    /// </summary>
    public class Script : BaseScript
    {

        [JsonPropertyName("slug")]
        public string Author { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
    #endregion

    #region [Robots]
    /// <summary>
    /// 机器人集合
    /// </summary>
    public class RobotCollection
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("items")]
        public List<Robot> Items { get; set; }
    }

    /// <summary>
    /// 机器人基本信息
    /// </summary>
    public class BaseRobot
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 机器人
    /// </summary>
    public class Robot : BaseRobot
    {
        [JsonPropertyName("online")]
        public bool Online { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; }
    }

    /// <summary>
    /// 修改机器人信息
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="tags"></param>
    public class ModifyRobot
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
    #endregion
}
