using System;

namespace HabitApp.Data
{
    public class HabitCompletion : ICloneable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public int HabitId { get; set; }

        public HabitCompletion(int rating, int habitId, DateTime date, int id = 0)
        {
            Id = id;
            Date = date;
            Rating = rating;
            HabitId = habitId;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
