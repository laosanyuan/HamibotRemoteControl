using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Common;
using HamibotRemoteControl.Core;
using HamibotRemoteControl.Core.ConfigManagers;
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
        /// <summary>
        /// 是否自动刷新
        /// </summary>
        [ObservableProperty]
        private bool _isAutoRefresh;
        #endregion

        public ShortcutSchemeViewModel()
        {
            WeakReferenceMessenger.Default.Register<object, string>(this,
                MessengerTokens.RefreshShortcutSchemes, async (_, obj) =>
                {
                    if (SettingsManager.CurrentSettings == null)
                    {
                        await SettingsManager.LoadConfig();
                    }

                    // 自动刷新每次返回首页都会发起请求
                    if (IsAutoRefresh)
                    {
                        await this.Refresh();
                    }
                    else
                    {
                        await this.UpdateSchemes();
                    }

                    this.IsAutoRefresh = UserCenter.Instance.AutoRefresh;
                });

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
        private async Task RunScheme(ShortcutSchemeModel scheme)
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
        private async Task StopScheme(ShortcutSchemeModel scheme)
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
        private async Task DeleteScheme(ShortcutSchemeModel scheme)
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
        /// 刷新数据
        /// </summary>
        [RelayCommand]
        private async Task Refresh()
        {
            // 对获取结果进行自然排序
            var robots = (await HamibotApi.GetRobotList())?.OrderBy(t => t.Name, new NaturalSortComparer()).ToList();

            var scripts = new List<Script>();
            if ((UserCenter.Instance.UserScriptType & ScriptType.Common) == ScriptType.Common)
            {
                var tmpCommon = await HamibotApi.GetScriptList();
                if (tmpCommon != null)
                {
                    tmpCommon.ForEach(t => scripts.Add(t));
                }
            }
            if ((UserCenter.Instance.UserScriptType & ScriptType.Developer) == ScriptType.Developer)
            {
                var tmpCommon = await HamibotApi.GetScriptList(ScriptType.Developer);
                if (tmpCommon != null)
                {
                    tmpCommon.ForEach(t => scripts.Add(t));
                }
            }

            bool isSuccess = true;
            if (robots != null)
            {
                await _robotDb.UpdateRobots(robots);
            }
            else
            {
                isSuccess = false;
                ToastHelper.Show("没有获取到有效机器人列表，请重试或者检查配置！");
            }

            if (scripts.Count > 0)
            {
                await _scriptDb.UpdateRobots(scripts);
            }
            else
            {
                isSuccess = false;
                ToastHelper.Show("没有获取到有效脚本列表，请重试或者检查配置！");
            }

            await UpdateSchemes();

            if (isSuccess)
            {
                ToastHelper.Show("数据刷新完成");
            }
        }

        /// <summary>
        /// 编辑快捷方案
        /// </summary>
        /// <param name="scheme"></param>
        [RelayCommand]
        private async Task EditScheme(ShortcutSchemeModel scheme)
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

            for (int i = schemes.Count - 1; i >= 0; i--)
            {
                var scheme = schemes[i];

                // 移除无效方案 
                // TODO 脚本不存在时应该标记无效不是直接删除，允许失效后修改
                if (scheme == null)
                {
                    schemes.RemoveAt(i);
                    await _shortcutSchemeDb.DeleteScheme(scheme.Id);
                    continue;
                }

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
