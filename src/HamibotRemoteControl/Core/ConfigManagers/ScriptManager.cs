using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Core.ConfigManagers
{
    internal static class ScriptManager
    {
        public static async Task<List<Script>> Load()
        {
            return await ConfigHelper.LoadConfig<List<Script>>(AppPath.ScriptsPath);
        }

        public static async Task<bool> Save(IList<Script> scripts)
        {
            return await ConfigHelper.SaveConfig(AppPath.ScriptsPath, scripts);
        }
    }
}
