using HamibotRemoteControl.Models;
using System.Text.Json;

namespace HamibotRemoteControl.Tools
{
    internal static class ConfigManager
    {
        /// <summary>
        /// 当前配置
        /// </summary>
        public static Settings CurrentSettings { get; private set; }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static async Task LoadConfig()
        {
            Settings result = default!;
            var filePath = AppPath.SettingPath;
            try
            {
                if (File.Exists(filePath))
                {
                    var text = await File.ReadAllTextAsync(filePath);
                    result = JsonSerializer.Deserialize<Settings>(text);
                }
            }
            catch (Exception e)
            {
                // 反序列化错误
            }

            if (result == null)
            {
                result = new Settings();
                await SaveConfig(result);
            }

            CurrentSettings = result;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<bool> SaveConfig(Settings config)
        {
            try
            {
                CurrentSettings = config;
                var filePath = AppPath.SettingPath;
                await using var writer = File.CreateText(filePath);
                var json = JsonSerializer.Serialize(config);
                await writer.WriteLineAsync(json);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
