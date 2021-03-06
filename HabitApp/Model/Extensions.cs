using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace HabitApp.Model
{
    public static class Extensions
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }

    public class IntTypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int valueInt)
            {
                if (parameter.ToString() == "1" && valueInt == 1) return true;
                else if (parameter.ToString() == "2" && valueInt == 2) return true;
                else if (parameter.ToString() == "3" && valueInt == 3) return true;
                else return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return int.Parse(parameter.ToString());
            }
            return 1;
        }
    }

    public class DateValidating : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out _)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Invalid date");
        }
    }
}
