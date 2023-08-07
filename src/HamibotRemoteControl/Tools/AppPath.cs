namespace HamibotRemoteControl.Tools
{
    static class AppPath
    {
        /// <summary>
        /// 设置文件路径
        /// </summary>
        public static string SettingPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "hamibot_settings.json");

        /// <summary>
        /// 数据库文件夹
        /// </summary>
        public static string DataBaseFolder = CreatePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db"));


        // 创建文件夹
        private static string CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
