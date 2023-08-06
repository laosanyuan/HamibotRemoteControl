using CommunityToolkit.Maui.Alerts;

namespace HamibotRemoteControl.Tools
{
    internal static class ToastHelper
    {
        public static void Show(string text)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Toast.Make(text).Show();
            });
        }
    }
}
