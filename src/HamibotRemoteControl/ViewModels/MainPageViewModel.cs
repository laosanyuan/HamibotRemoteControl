using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HamibotRemoteControl.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Core;
using HamibotRemoteControl.Core.ConfigManagers;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Common;
using HamibotRemoteControl.Tools;
using System.ComponentModel;

namespace HamibotRemoteControl.ViewModels
{
    /// <summary>
    /// 首页ViewModel
    /// </summary>
    [ObservableObject]
    partial class MainPageViewModel
    {

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
                MessengerTokens.RefreshMainPageData,
                (_, obj) => this.UpdateDisplayData());
        }

        #region [Commands]
        [RelayCommand]
        private async void Run()
        {
            if (this.SelectedScript == null)
            {
                //TODO Toast
                return;
            }

            var selectedRobots =
                this.Robots
                    .Where(t => t.IsSelected)
                    .Select(r => new BaseRobot() { Name = r.Name, Id = r.Id })
                    .ToList();
            if (!selectedRobots.Any())
            {
                //TODO Toast
                return;
            }

            if (!await HamibotApi.OperateScript(this.SelectedScript.Id, selectedRobots, true, this.SelectedScript.Type))
            {
                //TODO Toast
            }

            ClearSelected();
            ClearSelected();
        }

        [RelayCommand]
        private async void Stop()
        {
            if (this.SelectedScript == null)
            {
                //TODO Toast
                return;
            }

            var selectedRobots =
                this.Robots
                    .Where(t => t.IsSelected)
                    .Select(r => new BaseRobot() { Name = r.Name, Id = r.Id })
                    .ToList();
            if (!selectedRobots.Any())
            {
                //TODO Toast
                return;
            }

            if (!await HamibotApi.OperateScript(this.SelectedScript.Id, selectedRobots, false, this.SelectedScript.Type))
            {
                //TODO Toast
            }

            ClearSelected();
        }

        /// <summary>
        /// 刷新机器人、脚本列表
        /// </summary>
        [RelayCommand]
        private async void Refresh()
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

            if (robots != null)
            {
                this.Robots = new ObservableCollection<Robot>(robots);
            }
            else
            {
                //TODO
            }

            if (scripts.Count > 0)
            {
                this.Scripts = scripts;
                this.SelectedScript = scripts?.FirstOrDefault();
            }
            else
            {
                // TODO 获取脚本失败
            }

            await RobotManager.Save(this.Robots);
            await ScriptManager.Save(this.Scripts);
        }
        #endregion

        #region [Methods]
        // 更新显示数据
        private async void UpdateDisplayData()
        {
            if (SettingsManager.CurrentSettings == null)
            {
                await SettingsManager.LoadConfig();
                this.Robots = new ObservableCollection<Robot>(await RobotManager.Load() ?? new List<Robot>());
                this.Scripts = new ObservableCollection<Script>(await ScriptManager.Load() ?? new List<Script>());
                if (this.Scripts?.Any() == true && this.SelectedScript == null)
                {
                    this.SelectedScript = this.Scripts[0];
                }
            }

            this.IsAutoRefresh = UserCenter.Instance.AutoRefresh;

            // 自动刷新每次返回首页都会发起请求
            if (IsAutoRefresh)
            {
                this.Refresh();
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
