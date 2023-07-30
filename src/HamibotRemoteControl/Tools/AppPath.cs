namespace HamibotRemoteControl.Tools
{
    static class AppPath
    {
        /// <summary>
        /// 设置文件路径
        /// </summary>
        public static string SettingPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "hamibot_settings5.json");

        /// <summary>
        /// 机器人文件路径
        /// </summary>
        public static string RobotsPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "hamibot_robots.json");

        /// <summary>
        /// 脚本文件路径
        /// </summary>
        public static string ScriptsPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "hamibot_scripts.json");

    }
}
