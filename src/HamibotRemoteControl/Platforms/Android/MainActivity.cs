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

        // 设置状态栏颜色
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            // 获取 colors.xml 中定义的颜色资源
            int colorPrimaryResId = Resource.Color.colorPrimary;
            // 将资源颜色转换为 Android.Graphics.Color 对象
            Android.Graphics.Color colorPrimary = new Android.Graphics.Color(Resources.GetColor(colorPrimaryResId, null));
            // 设置顶部状态栏背景色
            Window.SetStatusBarColor(colorPrimary);
        }


    }
}
