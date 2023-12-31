﻿using CommunityToolkit.Mvvm.ComponentModel;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Models.DataBase;
using SQLite;
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
    /// 脚本信息
    /// </summary>
    public partial class Script
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        /// <summary>
        /// 脚本名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// 脚本作者
        /// </summary>
        [JsonPropertyName("slug")]
        public string Author { get; set; }
        /// <summary>
        /// 脚本本版号
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// 脚本类别
        /// </summary>
        public ScriptType Type { get; set; }

        /// <summary>
        /// 是否被隐藏
        /// </summary>
        public bool IsHidden { get; set; }
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
        [PrimaryKey]
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 机器人
    /// </summary>
    [ObservableObject]
    public partial class Robot : BaseRobot
    {
        /// <summary>
        /// 是否在线
        /// </summary>

        [JsonPropertyName("online")]
        public bool Online { get; set; }
        /// <summary>
        /// 标签
        /// </summary>

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
        /// <summary>
        /// 手机品牌
        /// </summary>

        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        /// <summary>
        /// 手机型号
        /// </summary>

        [JsonPropertyName("model")]
        public string Model { get; set; }
        /// <summary>
        /// Hamibot版本号
        /// </summary>

        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; }

        private bool _isSelected;
        /// <summary>
        /// 是否被用户选中
        /// </summary>
        public bool IsSelected
        {
            get => this._isSelected;
            set => SetProperty(ref this._isSelected, value);
        }

        /// <summary>
        /// 是否被用户隐藏
        /// </summary>

        public bool IsHidden { get; set; }

        public RobotEntity ToRobotEntity()
        {
            return new RobotEntity()
            {
                Id = Id,
                Name = Name,
                Online = Online,
                TagStr = Tags is null ? null : string.Join("#", Tags),
                Brand = Brand,
                Model = Model,
                AppVersion = AppVersion,
                IsHidden = IsHidden,
            };
        }
    }

    /// <summary>
    /// 修改机器人信息
    /// </summary>
    public class ModifyRobot
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
    #endregion
}
