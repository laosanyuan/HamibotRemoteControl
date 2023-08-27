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
        // 页面激活
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
            DataHistoryType.All => "所有",
            DataHistoryType.Month => "本月",
            DataHistoryType.Week => "近一周"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}