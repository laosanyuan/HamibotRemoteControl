namespace HamibotRemoteControl.Tools
{
    static class AppPath
    {
        /// <summary>
        /// 主配置文件路径
        /// </summary>
        public static string SettingPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "hamibot_settings5.json");

    }
}
