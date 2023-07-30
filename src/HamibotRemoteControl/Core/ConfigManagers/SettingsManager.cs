using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Core.ConfigManagers
{
    internal static class SettingsManager
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
            var filePath = AppPath.SettingPath;

            var result = await ConfigHelper.LoadConfig<Settings>(filePath);
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
            CurrentSettings = config;
            return await ConfigHelper.SaveConfig(AppPath.SettingPath, config);
        }
    }
}
