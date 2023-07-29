namespace HamibotRemoteControl.Enums
{
    [Flags]
    public enum ScriptType
    {
        /// <summary>
        /// 常规
        /// </summary>
        Common = 0x01,
        /// <summary>
        /// 开发者
        /// </summary>
        Developer = 0x10,
    }
}
