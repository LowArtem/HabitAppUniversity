using System;

namespace HabitApp.Model
{
    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
