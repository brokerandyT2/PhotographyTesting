using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace Location.Photography.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Colors.Red : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrEmpty(stringValue);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            // Check if parameter is provided with format "TrueText|FalseText"
            if (parameter is string paramString && paramString.Contains("|"))
            {
                string[] textOptions = paramString.Split('|');
                if (textOptions.Length == 2)
                {
                    return boolValue ? textOptions[0] : textOptions[1];
                }
            }

            // Default return if no parameter or invalid parameter
            return boolValue ? "True" : "False";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}