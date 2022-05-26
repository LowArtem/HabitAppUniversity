using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.Data
{
    public class DailyHabit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Difficulty { get; set; } // 1-3
        public bool Status { get; set; }
        public DateTime Deadline { get; set; }
        public int UserId { get; set; }

        public DailyHabit(string name, string description, string category, int userId, DateTime deadline, int difficulty = 1, bool status = false, int id = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Difficulty = difficulty;
            Status = status;
            Deadline = deadline;
            UserId = userId;
        }
    }
}
