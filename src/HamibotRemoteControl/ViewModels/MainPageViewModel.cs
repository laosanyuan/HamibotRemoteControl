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
using System.ComponentModel;
using Autofac;

namespace HamibotRemoteControl.ViewModels
{
    /// <summary>
    /// 首页ViewModel
    /// </summary>
    [ObservableObject]
    partial class MainPageViewModel
    {
        private readonly RobotDb _robotDb;
        private readonly ScriptDb _scriptDb;

        #region [Properties]
        /// <summary>
        /// 是否自动刷新
        /// </summary>
        [ObservableProperty]
        private bool _isAutoRefresh;

        /// <summary>
        /// 脚本列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Script> _scripts;

        /// <summary>
        /// 选中脚本
        /// </summary>
        [ObservableProperty]
        private Script _selectedScript;

        private ObservableCollection<Robot> _robots;
        /// <summary>
        /// 机器人列表
        /// </summary>
        public ObservableCollection<Robot> Robots
        {
            get => _robots;
            set
            {
                if (_robots != value)
                {
                    // 清除旧数据订阅事件
                    if (_robots != null)
                    {
                        foreach (var robot in this._robots)
                        {
                            robot.PropertyChanged -= Robot_PropertyChanged;
                        }
                    }

                    _robots = value;
                    UpdateRobotSelectStatus();
                    foreach (var robot in _robots)
                    {
                        robot.PropertyChanged += this.Robot_PropertyChanged;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Robots)));
                }
            }
        }

        /// <summary>
        /// 是否存在被选中机器人
        /// </summary>
        [ObservableProperty]
        private bool _haveSelectedRobot;
        #endregion

        public MainPageViewModel()
        {
            WeakReferenceMessenger.Default.Register<object, string>(this,
                MessengerTokens.RefreshMainPageData, async (_, obj) => await this.UpdateDisplayData());

            this._robotDb = App.Container.Resolve<RobotDb>();
            this._scriptDb = App.Container.Resolve<ScriptDb>();
        }

        #region [Commands]
        [RelayCommand]
        private async Task Run()
        {
            if (this.SelectedScript == null)
            {
                ToastHelper.Show("没有选则运行脚本，请选择后再执行！");
                return;
            }

            var selectedRobots =
                this.Robots
                    .Where(t => t.IsSelected)
                    .Select(r => new BaseRobot() { Name = r.Name, Id = r.Id })
                    .ToList();

            if (!await HamibotApi.OperateScript(this.SelectedScript.Id, selectedRobots, true, this.SelectedScript.Type))
            {
                ToastHelper.Show("执行操作失败，请重试或检查API配额是否充足！");
            }
            else
            {
                ToastHelper.Show("运行脚本成功");
            }
            ClearSelected();
        }

        [RelayCommand]
        private async Task Stop()
        {
            if (this.SelectedScript == null)
            {
                ToastHelper.Show("没有选则运行脚本，请选择后再执行！");
                return;
            }

            var selectedRobots =
                this.Robots
                    .Where(t => t.IsSelected)
                    .Select(r => new BaseRobot() { Name = r.Name, Id = r.Id })
                    .ToList();

            if (!await HamibotApi.OperateScript(this.SelectedScript.Id, selectedRobots, false, this.SelectedScript.Type))
            {
                ToastHelper.Show("执行操作失败，请重试或检查API配额是否充足！");
            }
            else
            {
                ToastHelper.Show("停止脚本成功");
            }

            ClearSelected();
        }

        /// <summary>
        /// 刷新机器人、脚本列表
        /// </summary>
        [RelayCommand]
        private async Task Refresh()
        {
            // 对获取结果进行自然排序
            var robots = (await HamibotApi.GetRobotList())?.OrderBy(t => t.Name, new NaturalSortComparer());

            var scripts = new ObservableCollection<Script>();
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
                this.Robots = new ObservableCollection<Robot>(robots);
            }
            else
            {
                isSuccess = false;
                ToastHelper.Show("没有获取到有效机器人列表，请重试或者检查配置！");
            }

            if (scripts.Count > 0)
            {
                this.Scripts = scripts;
                this.SelectedScript = scripts.FirstOrDefault();
            }
            else
            {
                isSuccess = false;
                ToastHelper.Show("没有获取到有效脚本列表，请重试或者检查配置！");
            }

            if (isSuccess)
            {
                ToastHelper.Show("数据刷新完成");
            }

            await _scriptDb.UpdateRobots(this.Scripts);
            await _robotDb.UpdateRobots(this.Robots);
        }
        #endregion

        #region [Methods]
        // 更新显示数据
        private async Task UpdateDisplayData()
        {
            if (SettingsManager.CurrentSettings == null)
            {
                this.Robots = new ObservableCollection<Robot>(await _robotDb.GetAllRobots());
                this.Scripts = new ObservableCollection<Script>(await _scriptDb.GetAllScripts());

                await SettingsManager.LoadConfig();
                if (this.Scripts?.Any() == true && this.SelectedScript == null)
                {
                    this.SelectedScript = this.Scripts[0];
                }
            }

            this.IsAutoRefresh = UserCenter.Instance.AutoRefresh;

            // 自动刷新每次返回首页都会发起请求
            if (IsAutoRefresh)
            {
                await this.Refresh();
            }
        }

        // 取消所有选中状态
        private void ClearSelected()
        {
            foreach (var robot in this.Robots)
            {
                robot.IsSelected = false;
            }
        }

        // 更新机器人选中状态
        private void UpdateRobotSelectStatus() => HaveSelectedRobot = Robots.Any(robot => !robot.IsHidden && robot.IsSelected);

        // 选中状态变化更新
        private void Robot_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Robot.IsSelected))
            {
                this.UpdateRobotSelectStatus();
            }
        }
        #endregion
    }
}
