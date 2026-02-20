using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SidebarNav.ViewModels;

namespace SidebarNav.Converters
{
    /// <summary>Bool → Visibility</summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = parameter != null && parameter.ToString().ToLower() == "invert";
            bool boolVal = value is bool b && b;
            if (invert) boolVal = !boolVal;
            return boolVal ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility v && v == Visibility.Visible;
        }
    }

    /// <summary>反转 Bool → Visibility</summary>
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility v && v == Visibility.Collapsed;
        }
    }

    /// <summary>Null / Empty → Visibility</summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = parameter != null && parameter.ToString().ToLower() == "invert";
            bool isNull = value == null || (value is string s && string.IsNullOrEmpty(s));
            if (invert) return isNull ? Visibility.Visible : Visibility.Collapsed;
            return isNull ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>BadgeType → Visibility（指定 parameter 为需要的 BadgeType 名称）</summary>
    public class BadgeTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BadgeType bt && parameter is string expected)
            {
                BadgeType expectedType;
                if (Enum.TryParse(expected, true, out expectedType))
                    return bt == expectedType ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>数字 > 0 → Visible</summary>
    public class GreaterThanZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i) return i > 0 ? Visibility.Visible : Visibility.Collapsed;
            if (value is double d) return d > 0 ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>宽度阈值 → 文本是否可见（用于展开/迷你模式切换时隐藏文本）</summary>
    public class WidthToTextVisibilityConverter : IValueConverter
    {
        public double Threshold { get; set; } = 100;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double w)
                return w > Threshold ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>层级 Level → 左缩进宽度（double，用于 Width 绑定）</summary>
    public class LevelToIndentConverter : IValueConverter
    {
        public double IndentPerLevel { get; set; } = 20;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int level = 0;
            if (value is int i) level = i;

            double indent = level * IndentPerLevel;

            // 如果目标是 Thickness 类型，返回 Margin
            if (targetType == typeof(Thickness))
                return new Thickness(indent, 0, 0, 0);

            // 否则返回 double（用于 Width）
            return indent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Bool 反转</summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }
    }

    /// <summary>IconType == 指定类型 → Visibility</summary>
    public class IconTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IconType it && parameter is string expected)
            {
                IconType expectedType;
                if (Enum.TryParse(expected, true, out expectedType))
                    return it == expectedType ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>多值绑定：搜索高亮时改变前景色</summary>
    public class SearchHighlightMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = IsSearchHighlighted, values[1] = 高亮色, values[2] = 默认色
            if (values.Length >= 3 && values[0] is bool highlighted && highlighted)
                return values[1];
            return values.Length >= 3 ? values[2] : DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
