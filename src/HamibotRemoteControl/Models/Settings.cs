using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using HamibotRemoteControl.Enums;

namespace HamibotRemoteControl.Models
{
    [ObservableObject]
    internal partial class Settings
    {
        /// <summary>
        /// Hamibot访问令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 是否自动刷新
        /// </summary>
        public bool AutoRefresh { get; set; }
        /// <summary>
        /// 脚本使用类别
        /// </summary>
        public ScriptType ScriptType { get; set; } = ScriptType.Common & ScriptType.Developer;

        /// <summary>
        /// 是否有效
        /// </summary>
        [JsonIgnore]
        public bool IsValid => !string.IsNullOrWhiteSpace(Token);
    }
}
