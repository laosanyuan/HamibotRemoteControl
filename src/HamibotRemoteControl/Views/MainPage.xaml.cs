using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        // 页面激活
        base.OnAppearing();
        WeakReferenceMessenger.Default.Send<object, string>(new object(), MessengerTokens.RefreshMainPageData);
    }
}

