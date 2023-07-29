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
        return (bool)value ? "是" : "否";
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
        var text = "常规脚本+开发者脚本";
        if ((type & ScriptType.Common) == ScriptType.Common)
        {
            text = "常规脚本";
        }
        else if ((type & ScriptType.Developer) == ScriptType.Developer)
        {
            text = "开发者脚本";
        }

        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var text = (string)value;
        return text switch
        {
            "常规脚本" => ScriptType.Common,
            "开发者脚本" => ScriptType.Developer,
            _ => ScriptType.Developer & ScriptType.Common,
        };
    }
}