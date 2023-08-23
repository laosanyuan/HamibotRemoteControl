using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.DataBase;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Interfaces;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;
using System.Collections.ObjectModel;

namespace HamibotRemoteControl.ViewModels
{
    [ObservableObject]
    internal partial class EditSchemePageViewModel : IViewModelReceiveData<string>
    {
        private readonly ShortcutSchemeDb _shortcutSchemeDb;
        private readonly ScriptDb _scriptDb;
        private readonly RobotDb _robotDb;

        // 编辑方案
        private ShortcutSchemeModel _editScheme;

        #region [Properties]
        /// <summary>
        /// 是否处于可编辑状态
        /// </summary>
        [ObservableProperty]
        private bool _canEdit;
        /// <summary>
        /// 编辑名称
        /// </summary>
        [ObservableProperty]
        private string _name;
        /// <summary>
        /// 获取机器人方式
        /// </summary>
        [ObservableProperty]
        private SelectRobotType _robotType;
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
        private ObservableCollection<SelectItem<Robot>> _robots;
        /// <summary>
        /// tag列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<SelectItem<string>> _tags;
        #endregion

        public EditSchemePageViewModel()
        {
            this._shortcutSchemeDb = App.Container.Resolve<ShortcutSchemeDb>();
            this._scriptDb = App.Container.Resolve<ScriptDb>();
            this._robotDb = App.Container.Resolve<RobotDb>();
        }

        #region [Commands]
        [RelayCommand]
        private async void Cancel()
        {
            await this.Close();
        }

        [RelayCommand]
        private async void Save()
        {
            try
            {
                if (this.CheckValid())
                {
                    await UpdateScheme();
                    // 通知状态变更
                    WeakReferenceMessenger.Default.Send(new object(), MessengerTokens.RefreshShortcutSchemes);
                    await this.Close();
                }

            }
            catch (Exception ex)
            {
                ToastHelper.Show("保存失败：" + ex.Message);
            }
        }

        #endregion

        #region [Public Methods]

        public async void ReceiveData(string id)
        {
            this.CanEdit = true;
            await ReloadData();
            if (string.IsNullOrEmpty(id))
            {
                this._editScheme = null;
            }
            else
            {
                this._editScheme = await _shortcutSchemeDb.GetScheme(id);
                this.Name = _editScheme.Name;
                this.RobotType = _editScheme.SelectRobotType;
                this.SelectedScript = this.Scripts.FirstOrDefault(t => t.Id.Equals(_editScheme.ScriptId));

                // 更新机器人、tag选中状态
                if (RobotType == SelectRobotType.Name && _editScheme.RobotIds != null)
                {
                    foreach (var robot in this.Robots)
                    {
                        if (_editScheme.RobotIds.Any(t => t.Equals(robot.Item.Id)))
                        {
                            robot.IsSelected = true;
                        }
                    }
                }
                else if (RobotType == SelectRobotType.Tag && _editScheme.Tags != null)
                {
                    foreach (var tag in this.Tags)
                    {
                        if (_editScheme.Tags.Any(t => t.Equals(tag.Item)))
                        {
                            tag.IsSelected = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region [Private Methods]
        // 更新数据
        private async Task ReloadData()
        {
            // 更新机器人列表
            var tmpRobotCollection = new ObservableCollection<SelectItem<Robot>>();
            var tmpRobots = await _robotDb.GetAllRobots();
            foreach (var robot in tmpRobots)
            {
                tmpRobotCollection.Add(new SelectItem<Robot>() { Item = robot });
            }
            this.Robots = tmpRobotCollection;

            // 更新脚本列表
            this.Scripts = new ObservableCollection<Script>(await _scriptDb.GetAllScripts());
            this.SelectedScript = this.Scripts.FirstOrDefault();

            // 更新tag列表
            var tmpTagCollection = new ObservableCollection<SelectItem<string>>();
            var tmpTags = await _robotDb.GetAllTags();
            foreach (var tag in tmpTags)
            {
                tmpTagCollection.Add(new SelectItem<string>() { Item = tag });
            }
            this.Tags = tmpTagCollection;

            this.RobotType = SelectRobotType.Name;
            this.Name = "新建快捷方案";
        }

        // 更新数据
        private async Task UpdateScheme()
        {
            // 更新编辑结果
            var result = new ShortcutSchemeModel()
            {
                Name = this.Name,
                Id = this._editScheme == null ? Guid.NewGuid().ToString() : this._editScheme.Id,
                ScriptId = this.SelectedScript.Id,
                SelectRobotType = this.RobotType
            };

            if (RobotType == SelectRobotType.Name)
            {
                result.RobotIds = this.Robots
                    .Where(t => t.IsSelected)
                    .Select(t => t.Item.Id)
                    .ToList();
            }
            else
            {
                result.Tags = this.Tags
                    .Where(t => t.IsSelected)
                    .Select(t => t.Item)
                    .ToList();
            }

            //更新数据库
            if (this._editScheme == null)
            {
                await this._shortcutSchemeDb.InsertScheme(result);
            }
            else
            {
                await this._shortcutSchemeDb.UpdateScheme(result);
            }
        }

        private async Task Close()
        {
            this.CanEdit = false;
            await Shell.Current.GoToAsync("///ShortcutScheme");
        }

        // 检查编辑有效性
        private bool CheckValid()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                ToastHelper.Show("保存失败，方案名称不可为空");
                return false;
            }
            else if (this.SelectedScript == null)
            {
                ToastHelper.Show("保存失败，必须选择一个脚本作为执行目标");
                return false;
            }
            else if (this.RobotType == SelectRobotType.Name && !this.Robots.Any(t => t.IsSelected))
            {
                ToastHelper.Show("保存失败，必须选择至少一个机器人");
                return false;
            }
            else if (this.RobotType == SelectRobotType.Tag && !this.Tags.Any(t => t.IsSelected))
            {
                ToastHelper.Show("保存失败，必须至少选择一个Tag");
                return false;
            }

            return true;
        }

        #endregion
    }

    [ObservableObject]

    public partial class SelectItem<T>
    {
        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private T _item;
    }
}
