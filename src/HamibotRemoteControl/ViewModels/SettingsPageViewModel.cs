using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.ViewModels
{
    /// <summary>
    /// 设置页ViewModel
    /// </summary>
    [ObservableObject]
    partial class SettingsPageViewModel
    {
        private Settings _settings;

        #region [Properties]
        /// <summary>
        /// 设置状态
        /// </summary>
        [ObservableProperty]
        private bool _isSetting;

        /// <summary>
        /// 令牌
        /// </summary>
        [ObservableProperty]
        private string _token;

        /// <summary>
        /// 是否自动刷新
        /// </summary>
        [ObservableProperty]
        private bool _autoRefresh;

        /// <summary>
        /// 配置是否有效
        /// </summary>
        [ObservableProperty]
        private bool _isValid;

        /// <summary>
        /// 使用脚本类型
        /// </summary>
        [ObservableProperty]
        private ScriptType _scriptType;
        #endregion

        public SettingsPageViewModel()
        {
            RefreshSettings();
        }

        #region [Commands]
        [RelayCommand]
        private void Modify()
        {
            this.IsSetting = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            this.RefreshSettings();
            this.IsSetting = false;
        }

        [RelayCommand]
        private async void Save()
        {
            Settings tmpSettings = new Settings()
            {
                Token = this.Token,
                AutoRefresh = this.AutoRefresh,
                ScriptType = this.ScriptType,
            };
            await ConfigManager.SaveConfig(tmpSettings);
            this.RefreshSettings();
            this.IsSetting = false;
        }
        #endregion

        private void RefreshSettings()
        {
            this._settings = ConfigManager.CurrentSettings;

            this.Token = this._settings.Token;
            this.AutoRefresh = this._settings.AutoRefresh;
            this.ScriptType = this._settings.ScriptType;
            this.IsValid = this._settings.IsValid;
        }
    }
}
