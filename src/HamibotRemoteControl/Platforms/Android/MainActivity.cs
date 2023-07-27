using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace HamibotRemoteControl;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // 请求外部存储权限
        RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);

    }
}
