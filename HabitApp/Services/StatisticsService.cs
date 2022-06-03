using HabitApp.Implementation;
using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.Services
{
    public class StatisticsService
    {
        private readonly TaskRepository _taskRepository;
        private readonly HabitRepository _habitRepository;
        private readonly DailyHabitRepository _dailyHabitRepository;
        private readonly StatisticsRepository _statisticsRepository;

        public StatisticsService(TaskRepository taskRepository, HabitRepository habitRepository, DailyHabitRepository dailyHabitRepository, StatisticsRepository statisticsRepository)
        {
            _taskRepository = taskRepository;
            _habitRepository = habitRepository;
            _dailyHabitRepository = dailyHabitRepository;
            _statisticsRepository = statisticsRepository;
        }

        public enum TypeOfPeriod
        {
            Day,
            Week,
            Month,
            Year
        }

        public int GetHabitsCompletionsCountForPeriod(int userId, TypeOfPeriod period, DateTime firstDayOfPeriod)
        {
            switch (period)
            {
                case TypeOfPeriod.Day: return _statisticsRepository.GetHabitsCompletionsCountForDay(userId, firstDayOfPeriod);
                case TypeOfPeriod.Week: return _statisticsRepository.GetHabitsCompletionsCountForWeek(userId, firstDayOfPeriod);
                case TypeOfPeriod.Month: return _statisticsRepository.GetHabitsCompletionsCountForMonth(userId, firstDayOfPeriod);
                case TypeOfPeriod.Year: return _statisticsRepository.GetHabitsCompletionsCountForYear(userId, firstDayOfPeriod);
                default: throw new ArgumentException("Wrong period");
            }
        }

        public int GetDailyHabitsCompletionsCountForPeriod(int userid, TypeOfPeriod period, DateTime firstDayOfPeriod)
        {
            switch (period)
            {
                case TypeOfPeriod.Day: return _statisticsRepository.GetDailyHabitsCompletionsCountForDay(userid, firstDayOfPeriod);
                case TypeOfPeriod.Week: return _statisticsRepository.GetDailyHabitsCompletionsCountForWeek(userid, firstDayOfPeriod);
                case TypeOfPeriod.Month: return _statisticsRepository.GetDailyHabitsCompletionsCountForMonth(userid, firstDayOfPeriod);
                case TypeOfPeriod.Year: return _statisticsRepository.GetDailyHabitsCompletionsCountForYear(userid, firstDayOfPeriod);
                default: throw new ArgumentException("Wrong period");
            }
        }

        public int GetTasksCompletionsCountForPeriod(int userid, TypeOfPeriod period, DateTime firstDayOfPeriod)
        {
            switch (period)
            {
                case TypeOfPeriod.Day: return _statisticsRepository.GetTasksCompletionsCountForDay(userid, firstDayOfPeriod);
                case TypeOfPeriod.Week: return _statisticsRepository.GetTasksCompletionsCountForWeek(userid, firstDayOfPeriod);
                case TypeOfPeriod.Month: return _statisticsRepository.GetTasksCompletionsCountForMonth(userid, firstDayOfPeriod);
                case TypeOfPeriod.Year: return _statisticsRepository.GetTasksCompletionsCountForYear(userid, firstDayOfPeriod);
                default: throw new ArgumentException("Wrong period");
            }
        }

        public List<DateTimePoint> GetAllRatingsWithDates(int userId)
        {
            var dataList = _statisticsRepository.GetAllRatingsWithDates(userId);
            var dataGrouped = dataList.GroupBy(x => x.DateTime.Date);

            var resultList = new List<DateTimePoint>();

            foreach (var date in dataGrouped)
            {
                var resultDate = date.Key;
                double resultRating = 0;
                int counter = 0;

                foreach (var rating in date)
                {
                    resultRating += (double)rating.Value;
                    counter++;
                }

                resultRating = resultRating / counter;

                resultList.Add(new DateTimePoint(resultDate, resultRating));
            }

            return resultList;
        }

        public int GetTotalCompletionsCount(int userId) => _statisticsRepository.GetTotalCompletionsCount(userId);

        public List<int> GetCurrentHabitsAndCompletedTodayHabits(int userId)
        {
            int allHabits = _habitRepository.GetAllByUserId(userId).Count();
            int todayCompleted = _statisticsRepository.GetTodayCompletedHabitsCount(userId);

            return new List<int> { allHabits, todayCompleted };
        }

        public List<int> GetCurrentTasksAndCompletedTasks(int userId)
        {
            int allTasks = _taskRepository.GetAllTasksByUser(userId).Count();
            int completed = _statisticsRepository.GetCurrentCompletedTasksCount(userId);

            return new List<int> { allTasks, completed };
        }
    }
}
