using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Core;
using HamibotRemoteControl.DataBase;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;
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
        /// <summary>
        /// 正在被编辑方案
        /// </summary>
        [ObservableProperty]
        private ShortcutSchemeModel _editingScheme;

        #endregion

        public ShortcutSchemeViewModel()
        {
            WeakReferenceMessenger.Default.Register<object, string>(this,
                MessengerTokens.RefreshShortcutSchemes, async (_, obj) => await this.UpdateSchemes());

            this._scriptDb = App.Container.Resolve<ScriptDb>();
            this._robotDb = App.Container.Resolve<RobotDb>();
            this._shortcutSchemeDb = App.Container.Resolve<ShortcutSchemeDb>();
        }

        #region [Icommands]
        /// <summary>
        /// 运行快捷方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private async void RunScheme(ShortcutSchemeModel scheme)
        {
            var robots = scheme.Robots;
            if (!scheme.IncludeHiddenRobot)
            {
                robots = robots.Where(t => t.Online).ToList();
            }

            var result = await HamibotApi.OperateScript(scheme.ScriptId, robots.Cast<BaseRobot>().ToList(), true, scheme.Script.Type);
            if (!result)
            {
                ToastHelper.Show("快捷方案失运行失败");
            }
        }

        /// <summary>
        /// 停止快捷方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private async void StopScheme(ShortcutSchemeModel scheme)
        {
            var robots = scheme.Robots;
            if (!scheme.IncludeHiddenRobot)
            {
                robots = robots.Where(t => t.Online).ToList();
            }

            var result = await HamibotApi.OperateScript(scheme.ScriptId, robots.Cast<BaseRobot>().ToList(), false, scheme.Script.Type);
            if (!result)
            {
                ToastHelper.Show("快捷方案失停止失败");
            }
        }

        /// <summary>
        /// 删除快捷方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private async void DeleteScheme(ShortcutSchemeModel scheme)
        {
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                "确认",
                "删除快捷方案后无法恢复，是否继续？",
                "确定",
                "取消");
            if (isConfirmed)
            {
                await this._shortcutSchemeDb.DeleteScheme(scheme.Id);
                await UpdateSchemes();
            }
        }

        /// <summary>
        /// 创建快捷方案
        /// </summary>
        [RelayCommand]
        private async void CreateScheme()
        {
            await Shell.Current.GoToAsync($"///EditSchemePage?param={Uri.EscapeDataString("")}");
        }

        /// <summary>
        /// 编辑快捷方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private async void EditScheme(ShortcutSchemeModel scheme)
        {
            await Shell.Current.GoToAsync($"///EditSchemePage?param={Uri.EscapeDataString(scheme.Id)}");
        }

        /// <summary>
        /// 置顶方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private void TopScheme(ShortcutSchemeModel scheme)
        {
            ToastHelper.Show("暂时还不支持置顶，敬请期待下个版本更新~");
        }

        #endregion

        #region [Private Methods]
        /// <summary>
        /// 更新方案集合
        /// </summary>
        /// <returns></returns>
        private async Task UpdateSchemes()
        {
            var schemes = await _shortcutSchemeDb.GetAllSchemes();

            if (schemes == null)
            {
                this.ShortcutSchemes = null;
                return;
            }

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
                    scheme.Robots = await this._robotDb.GetRobotsByTags(scheme.Tags, scheme.IncludeHiddenRobot);
                }
            }

            this.ShortcutSchemes = new ObservableCollection<ShortcutSchemeModel>(schemes);
        }
        #endregion
    }
}
