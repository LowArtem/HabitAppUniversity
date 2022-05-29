using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace HabitApp.Data
{
    public static class Categories
    {
        public const string Health = "Health";
        public const string Education = "Education";
        public const string Entertainment = "Entertainment";
        public const string SelfDevelopment = "Self-Development";
        public const string Household = "Household";
        public const string Work = "Work";
        public const string Other = "Other";

        public static List<string> GetAll()
        {
            var list = new List<string>();
            list.Add(Health);
            list.Add(Education);
            list.Add(Entertainment);
            list.Add(SelfDevelopment);
            list.Add(Household);
            list.Add(Work);
            list.Add(Other);

            return list;
        }
    }

    public class CategoryConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case Categories.Health: return 0;
                case Categories.Education: return 1;
                case Categories.Entertainment: return 2;
                case Categories.SelfDevelopment: return 3;
                case Categories.Household: return 4;
                case Categories.Work: return 5;
                case Categories.Other: return 6;
                default: return 0;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            switch (int.Parse(value.ToString()))
            {
                case 0: return Categories.Health;
                case 1: return Categories.Education;
                case 2: return Categories.Entertainment;
                case 3: return Categories.SelfDevelopment;
                case 4: return Categories.Household;
                case 5: return Categories.Work;
                case 6: return Categories.Other;
                default: return 0;
            }
        }
    }
}
