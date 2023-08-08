using Autofac;
using IContainer = Autofac.IContainer;

namespace HamibotRemoteControl.ViewModels
{
    class ViewModelLocator
    {
        private static ContainerBuilder _builder = new ContainerBuilder();
        private static IContainer _container;

        static ViewModelLocator()
        {
            _builder.RegisterType<MainPageViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<RobotManagePageViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<ScriptManagePageViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<DataStatisticsPageViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<SettingsPageViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<ShortcutSchemeViewModel>().AsSelf().SingleInstance();

            _container = _builder.Build();
        }

        public MainPageViewModel MainPage => _container.Resolve<MainPageViewModel>();
        public RobotManagePageViewModel RobotManagePage => _container.Resolve<RobotManagePageViewModel>();
        public ScriptManagePageViewModel ScriptManagePage => _container.Resolve<ScriptManagePageViewModel>();
        public DataStatisticsPageViewModel DataStatisticsPage => _container.Resolve<DataStatisticsPageViewModel>();
        public SettingsPageViewModel SettingsPage => _container.Resolve<SettingsPageViewModel>();
        public ShortcutSchemeViewModel ShortcutScheme => _container.Resolve<ShortcutSchemeViewModel>();
    }
}
