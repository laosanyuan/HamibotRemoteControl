using Autofac;
using HamibotRemoteControl.DataBase;
using HamibotRemoteControl.Tools;
using IContainer = Autofac.IContainer;

namespace HamibotRemoteControl;

public partial class App : Application
{
    public static IContainer Container;

    public App()
    {
        InitializeComponent();
        InitContainer();

        MainPage = new AppShell();

        Task.Run(CheckVersion);
    }


    public void InitContainer()
    {
        ContainerBuilder builder = new();

        builder.RegisterType<ApiCallCountDb>().AsSelf().SingleInstance();
        builder.RegisterType<RobotDb>().AsSelf().SingleInstance();
        builder.RegisterType<ScriptDb>().AsSelf().SingleInstance();
        builder.RegisterType<ShortcutSchemeDb>().AsSelf().SingleInstance();

        Container = builder.Build();
    }

    private async Task CheckVersion()
    {
        // 开启三分钟后开始检查
        await Task.Delay(1000 * 60 * 3);
        if (await Software.HaveNewVersion())
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                bool result = await App.Current.MainPage.DisplayAlert("有新版本", "发现作者已发布【Hamibot遥控器】新版本，是否下载新版本？", "去下载", "暂不处理");

                if (result)
                {
                    await Launcher.OpenAsync("https://github.com/laosanyuan/HamibotRemoteControl/releases");
                }
            });
        }
    }
}
