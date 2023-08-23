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
        // ҳ�漤��
        base.OnAppearing();
        WeakReferenceMessenger.Default.Send<object, string>(new object(), MessengerTokens.RefreshShortcutSchemes);
    }

    // �ö�
    private void TopClicked(object sender, EventArgs e)
    {
        // SwipeItemView��ItemTemplate�а󶨲��ɹ�������ôд
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).TopSchemeCommand.Execute(scheme);
        }
    }

    // �༭
    private void EditClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).EditSchemeCommand.Execute(scheme);
        }
    }

    // ɾ��
    private void DeleteClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is ShortcutSchemeModel scheme)
        {
            ((ShortcutSchemeViewModel)this.BindingContext).DeleteSchemeCommand.Execute(scheme);
        }
    }
}