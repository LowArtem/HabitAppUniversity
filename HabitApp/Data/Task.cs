using System;

namespace HabitApp.Data
{
    public class Task : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }
        public bool Status { get; set; }
        public int? GroupEventId { get; set; }
        public int? UserEventId { get; set; }

        public Task(string name, string description, DateTime deadline, int priority = 1, int difficulty = 1, int? groupEventId = null, int? userEventId = null, bool status = false, int id = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Priority = priority;
            Deadline = deadline;
            Status = status;
            GroupEventId = groupEventId;
            UserEventId = userEventId;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
