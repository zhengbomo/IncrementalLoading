/* ==============================================================================
 * 功能描述：VisibilityConverter  
 * 创 建 者：贤凯
 * 创建日期：1/29/2015 9:36:09 PM
 * ==============================================================================*/

using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IncrementalLoadingDemo.Converters
{
    /// <summary>
    /// 支持bool，null，zero转换
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var p = (string)parameter;

            switch (p)
            {
                case "bool":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                case "boolreverse":
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                case "zero":
                    return value.Equals(0) ? Visibility.Visible : Visibility.Collapsed;
                case "zeroreverse":
                    return value.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
                case "null":
                    return value == null ? Visibility.Visible : Visibility.Collapsed;
                case "nullreverse":
                    return value == null ? Visibility.Collapsed : Visibility.Visible;
                case "empty":
                    return string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;
                case "emptyreverse":
                    return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
                case "listempty":
                    return ((IList)value).Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                case "listemptyreverse":
                    return ((IList)value).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                default:
                    if (p.StartsWith("equal(") && p.EndsWith(")"))
                    {
                        return value.Equals(p.Substring(6, p.Length - 7)) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else if (p.StartsWith("equalreverse(") && p.EndsWith(")"))
                    {
                        return value.Equals(p.Substring(6, p.Length - 7)) ? Visibility.Collapsed : Visibility.Visible;
                    }
                    break;
            }

            throw new NotSupportedException("不支持其他值的转换");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
