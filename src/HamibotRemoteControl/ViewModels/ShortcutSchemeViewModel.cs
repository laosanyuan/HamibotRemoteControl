using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using HamibotRemoteControl.DataBase;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Models;
using System.Collections.ObjectModel;

namespace HamibotRemoteControl.ViewModels
{
    [ObservableObject]
    internal partial class ShortcutSchemeViewModel
    {
        private ShortcutSchemeDb _shortcutSchemeDb;
        private ScriptDb _scriptDb;
        private RobotDb _robotDb;

        #region [Properties]
        /// <summary>
        /// 快捷方案列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ShortcutSchemeModel> _shortcutSchemes;
        #endregion


        public ShortcutSchemeViewModel()
        {
            this._shortcutSchemeDb = App.Container.Resolve<ShortcutSchemeDb>();
            this._scriptDb = App.Container.Resolve<ScriptDb>();
            this._robotDb = App.Container.Resolve<RobotDb>();

            Task.Run(UpdateSchemes);
        }



        #region [Private Methods]
        /// <summary>
        /// 更新方案集合
        /// </summary>
        /// <returns></returns>
        private async Task UpdateSchemes()
        {
            var schemes = await _shortcutSchemeDb.GetAllSchemes();

            foreach (var scheme in schemes)
            {
                // 获取脚本信息
                scheme.Script = await this._scriptDb.GetScript(scheme.ScriptId);
                // 获取机器人信息
                if (scheme.SelectRobotType == SelectRobotType.Name)
                {
                    scheme.Robots = await this._robotDb.GetRobotByIds(scheme.RobotIds);
                }
                else
                {
                    scheme.Robots = await this._robotDb.GetRobotsByTags(scheme.Tags);
                }
            }

            this.ShortcutSchemes = new ObservableCollection<ShortcutSchemeModel>(schemes);
        }
        #endregion
    }
}
