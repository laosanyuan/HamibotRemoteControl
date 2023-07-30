using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HamibotRemoteControl.Models;
using System.Collections.ObjectModel;

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

        /// <summary>
        /// 机器人列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Robot> _robots;

        /// <summary>
        /// 是否可执行操作
        /// </summary>
        [ObservableProperty]
        private bool _canOperate;
        #endregion

        public MainPageViewModel()
        {
            this.Robots = new ObservableCollection<Robot>()
            {
                new Robot() { Name = "test", Id = "137891238791892389731289713289", Online = true,Tags = new List<string>(){"esssss","esssss","esstrtsss","esssewqeqss","esssss","esssss","esssss","esssssssss",}},
                new Robot() { Name = "test" ,Id = "137891238791892389731289713289",Online = false, Brand="google" , Model="Pixel 2 XL",AppVersion="Hamibot 1.5.0-beta" },
                new Robot() { Name = "test" ,Id = "137891238791892389731289713289",Online = false, Brand="google" , Model="Pixel 2 XL",AppVersion="Hamibot 1.5.0-beta" },
                new Robot() { Name = "test" ,Id = "137891238791892389731289713289",Online = false, Brand="google" , Model="Pixel 2 XL",AppVersion="Hamibot 1.5.0-beta" },
                new Robot() { Name = "test", Id = "137891238791892389731289713289", Online = true,Tags = new List<string>(){"esssss","esssss","esstrtsss","esssewqeqss","esssss","esssss","esssss","esssssssss",}},
                new Robot() { Name = "test" },
            };

            this.Scripts = new ObservableCollection<Script>()
            {
                new Script() { Name = "test" },
                new Script() { Name = "test" },
            };
        }

        #region [Commands]
        [RelayCommand]
        private void Run()
        {
            ClearSelected();
        }

        [RelayCommand]
        private void Stop()
        {
            ClearSelected();
        }

        [RelayCommand]
        private void Refresh()
        {
            ClearSelected();
        }
        #endregion

        private void ClearSelected()
        {
            foreach (var robot in this.Robots)
            {
                robot.IsSelected = false;
            }
        }
    }
}
