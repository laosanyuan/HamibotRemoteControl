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

    // ����·����Ϣ
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
/// ѡ�з�ʽת��Ϊ�ַ���
/// </summary>
public class SelectRobotTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SelectRobotType and SelectRobotType.Tag)
        {
            return "��Tagѡ��";
        }

        return "������������ѡ��";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text && text.Equals("��Tagѡ��"))
        {
            return SelectRobotType.Tag;
        }

        return SelectRobotType.Name;
    }
}

/// <summary>
/// ѡ�з�ʽת����
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