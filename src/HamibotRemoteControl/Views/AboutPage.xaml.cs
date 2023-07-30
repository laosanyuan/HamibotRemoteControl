using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        this.version.Text = $"HamibotÒ£¿ØÆ÷ - {Software.Version}";
    }

    private async void OnLabelTapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://hamibot.com/referrals/m7ax"));
    }

    private async void OnGithubTapped(object sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://github.com/laosanyuan/HamibotRemoteControl"));
    }
}