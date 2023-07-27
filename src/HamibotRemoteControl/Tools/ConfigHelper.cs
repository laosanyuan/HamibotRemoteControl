using System.Text.Json;

namespace HamibotRemoteControl.Tools
{
    internal static class ConfigHelper
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadConfig<T>(string filePath)
        {
            T result = default;
            try
            {
                if (File.Exists(filePath))
                {
                    var text = File.ReadAllText(filePath);
                    result = JsonSerializer.Deserialize<T>(text);
                }
            }
            catch (Exception e)
            {
                // 反序列化错误
            }

            return result;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<bool> SaveConfig<T>(string filePath, T config)
        {
            try
            {
                await using var writer = File.CreateText(filePath);
                await writer.WriteLineAsync(JsonSerializer.Serialize(config));
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
