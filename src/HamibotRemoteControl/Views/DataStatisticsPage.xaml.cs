using System.Globalization;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Tools;

namespace HamibotRemoteControl.Views;

public partial class DataStatisticsPage : ContentPage
{
    public DataStatisticsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        // ҳ�漤��
        base.OnAppearing();
        WeakReferenceMessenger.Default.Send<object, string>(new object(), MessengerTokens.RefreshDataStatisticPageData);
    }
}

public class DataHistoryToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var type = (DataHistoryType)value;
        return type switch
        {
            DataHistoryType.All => "����",
            DataHistoryType.Month => "����",
            DataHistoryType.Week => "��һ��"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}