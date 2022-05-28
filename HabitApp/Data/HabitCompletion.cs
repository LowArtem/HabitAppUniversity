using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.Data
{
    public class HabitCompletion
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
    }
}
