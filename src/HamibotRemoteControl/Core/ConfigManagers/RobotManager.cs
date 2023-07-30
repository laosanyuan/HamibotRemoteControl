using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Core.ConfigManagers
{
    internal class RobotManager
    {
        public static async Task<List<Robot>> Load()
        {
            return await ConfigHelper.LoadConfig<List<Robot>>(AppPath.RobotsPath);
        }

        public static async Task<bool> Save(IList<Robot> robots)
        {
            return await ConfigHelper.SaveConfig(AppPath.RobotsPath, robots);
        }
    }
}
