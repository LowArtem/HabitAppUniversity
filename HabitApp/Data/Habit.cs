using System;

namespace HabitApp.Data
{
    public class Habit : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Type { get; set; } // 1+ 2- 3+-
        public int Difficulty { get; set; } // 1-3
        public int UserId { get; set; }

        public Habit(string name, string description, string category, int userId, int type = 1, int difficulty = 1, int id = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Type = type;
            Difficulty = difficulty;
            UserId = userId;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
