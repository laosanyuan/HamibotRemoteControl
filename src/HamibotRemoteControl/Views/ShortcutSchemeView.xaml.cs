using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Views;

public partial class ShortcutSchemeView : ContentPage
{
    public ShortcutSchemeView()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        // Ò³Ãæ¼¤»î
        base.OnAppearing();
        WeakReferenceMessenger.Default.Send<object, string>(new object(), MessengerTokens.RefreshShortcutSchemes);
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        this.add_window.IsVisible = true;
    }
}