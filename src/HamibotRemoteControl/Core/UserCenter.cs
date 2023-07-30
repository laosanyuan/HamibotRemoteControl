using HamibotRemoteControl.Core.ConfigManagers;
using HamibotRemoteControl.Enums;

namespace HamibotRemoteControl.Core
{
    class UserCenter
    {
        private static UserCenter _instance = new UserCenter();
        public static UserCenter Instance => _instance;


        public string Token => SettingsManager.CurrentSettings.Token;
        public bool AutoRefresh => SettingsManager.CurrentSettings.AutoRefresh;
        public ScriptType UserScriptType => SettingsManager.CurrentSettings.ScriptType;

        private UserCenter()
        {
        }
    }
}
