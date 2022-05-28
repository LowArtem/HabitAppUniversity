using System;
using System.ComponentModel;

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
}
