using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        this.version.Text = $"版本号：{Software.Version}";
    }
}
