using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Views;

public partial class ShortcutSchemeView : ContentPage
{
    // ����ɨ��
    private Frame _swiped;

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

    private void Button_OnClicked(object sender, EventArgs e)
    {
        this.add_window.IsVisible = true;
    }

    // �����ݷ�����Ƭ����
    private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
    {
        if (sender is Frame frame)
        {
            switch (e.Direction)
            {
                // ֻ����һ����������
                case SwipeDirection.Left when this._swiped == frame:
                    return;
                case SwipeDirection.Left:
                    if (this._swiped != null)
                    {
                        this._swiped.Margin = new Thickness(0, 0, 0, 15);
                    }
                    frame.Margin = new Thickness(-155, 0, 155, 15);
                    this._swiped = frame;
                    break;
                case SwipeDirection.Right:
                    frame.Margin = new Thickness(0, 0, 0, 15);
                    if (frame == _swiped)
                    {
                        this._swiped = null;
                    }
                    break;
            }
        }
    }
}