using System;

namespace HabitApp.Model
{
    /// <summary>
    /// Интерфейс для получения текущей даты и времени
    /// </summary>
    public interface ICurrentDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
