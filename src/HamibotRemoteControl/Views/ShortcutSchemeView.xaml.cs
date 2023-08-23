using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Models;
using HamibotRemoteControl.Tools;
using HamibotRemoteControl.ViewModels;

namespace HamibotRemoteControl.Views;

public partial class ShortcutSchemeView : ContentPage
{
    public ShortcutSchemeView()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        // 页面激活
        base.OnAppearing();
        WeakReferenceMessenger.Default.Send<object, string>(new object(), MessengerTokens.RefreshShortcutSchemes);
    }

    // 置顶
    private void TopClicked(object sender, EventArgs e)
    {
        // SwipeItemView在ItemTemplate中绑定不成功，先这么写
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).TopSchemeCommand.Execute(scheme);
        }
    }

    // 编辑
    private void EditClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).EditSchemeCommand.Execute(scheme);
        }
    }

    // 删除
    private void DeleteClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).DeleteSchemeCommand.Execute(scheme);
        }
    }
}