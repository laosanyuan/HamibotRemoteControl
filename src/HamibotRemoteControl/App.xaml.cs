using Autofac;
using HamibotRemoteControl.DataBase;
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
    }


    public void InitContainer()
    {
        ContainerBuilder builder = new();

        builder.RegisterType<ApiCallCountDb>().AsSelf().SingleInstance();
        builder.RegisterType<RobotDb>().AsSelf().SingleInstance();
        builder.RegisterType<ScriptDb>().AsSelf().SingleInstance();

        Container = builder.Build();
    }
}
