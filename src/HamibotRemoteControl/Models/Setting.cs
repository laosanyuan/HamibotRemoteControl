namespace HamibotRemoteControl.Models
{
    class Setting
    {
        /// <summary>
        /// Hamibot访问令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 是否自动刷新
        /// </summary>
        public bool AutoRefresh { get; set; }
    }
}
