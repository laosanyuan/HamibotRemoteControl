using System.Text.Json;

namespace HamibotRemoteControl.Tools
{
    internal static class ConfigHelper
    {
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<bool> SaveConfig<T>(string filePath, T config)
        {
            try
            {
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


        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<T> LoadConfig<T>(string filePath)
        {
            T result = default!;
            try
            {
                if (File.Exists(filePath))
                {
                    var text = await File.ReadAllTextAsync(filePath);
                    result = JsonSerializer.Deserialize<T>(text);
                }
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}
