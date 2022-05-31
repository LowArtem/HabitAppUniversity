using System;

namespace HabitApp.Data
{
    public class HabitCompletion : ICloneable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public int HabitId { get; set; }
        public bool IsPositive { get; set; }
        public bool IsNegative { get => !IsPositive; }

        public HabitCompletion(int rating, int habitId, DateTime date, bool isPositive = true, int id = 0)
        {
            Id = id;
            Date = date;
            Rating = rating;
            HabitId = habitId;
            IsPositive = isPositive;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
