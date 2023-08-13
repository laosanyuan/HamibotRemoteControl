using System.Globalization;
using HamibotRemoteControl.Interfaces;
using System.Web;
using HamibotRemoteControl.Enums;

namespace HamibotRemoteControl.Views;

public partial class EditSchemePage : ContentPage, IQueryAttributable
{
    public EditSchemePage()
    {
        InitializeComponent();
    }

    // 接受路由消息
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var param = HttpUtility.UrlDecode(query["param"].ToString());
        if (this.BindingContext is IViewModelReceiveData<string> datacontext)
        {
            datacontext.ReceiveData(param);
        }
    }
}

/// <summary>
/// 选中方式转换为字符串
/// </summary>
public class SelectRobotTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SelectRobotType and SelectRobotType.Tag)
        {
            return "按Tag选择";
        }

        return "按机器人名称选择";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text && text.Equals("按Tag选择"))
        {
            return SelectRobotType.Tag;
        }

        return SelectRobotType.Name;
    }
}

/// <summary>
/// 选中方式转布尔
/// </summary>
public class SelectRobotTypeToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SelectRobotType type && parameter is SelectRobotType para)
        {
            if (type == para)
            {
                return true;
            }
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}