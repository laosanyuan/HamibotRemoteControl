using System.Globalization;
using HamibotRemoteControl.Enums;

namespace HamibotRemoteControl.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }
}

internal class BooleanTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "��" : "��";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ScriptTypeTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var type = (ScriptType)value;
        var text = "����ű�+�����߽ű�";
        if ((type & ScriptType.Common) == ScriptType.Common)
        {
            text = "����ű�";
        }
        else if ((type & ScriptType.Developer) == ScriptType.Developer)
        {
            text = "�����߽ű�";
        }

        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var text = (string)value;
        return text switch
        {
            "����ű�" => ScriptType.Common,
            "�����߽ű�" => ScriptType.Developer,
            _ => ScriptType.Developer & ScriptType.Common,
        };
    }
}