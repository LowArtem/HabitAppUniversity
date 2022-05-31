using HabitApp.Data;
using HabitApp.Implementation;
using HabitApp.Model;
using System.Collections.Generic;

namespace HabitApp.Services
{
    public class AllHabitService
    {
        private readonly HabitRepository _habitRepository;
        private readonly DailyHabitRepository _dailyHabitRepository;
        private readonly TaskRepository _taskRepository;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public AllHabitService(HabitRepository habitRepository, DailyHabitRepository dailyHabitRepository, TaskRepository taskRepository, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _habitRepository = habitRepository;
            _dailyHabitRepository = dailyHabitRepository;
            _taskRepository = taskRepository;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public List<HabitCompletion> GetHabitCompletions(int habitId) => _habitRepository.GetAllCompletionsById(habitId);
        public List<HabitCompletion> GetDailyHabitCompletions(int dailyHabitId) => _dailyHabitRepository.GetAllCompletionsById(dailyHabitId);

        public void ChangeTaskCompletedStatus(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Status = !task.Status;
            _taskRepository.Update(task);
        }

        public void AddDailyHabitCompletion(int dailyHabitId, int rating)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Status = !dailyHabit.Status;
            _dailyHabitRepository.Update(dailyHabit);

            _dailyHabitRepository.AddHabitCompletion(dailyHabitId, _currentDateTimeProvider.GetCurrentDateTime(), rating);
        }

        public void AddHabitCompletion(int habitId, int rating, bool isPositive = true)
        {
            _habitRepository.AddHabitCompletion(habitId, _currentDateTimeProvider.GetCurrentDateTime(), rating, isPositive);
        }

        public void DeleteHabitCompletion(int habitCompletionId)
        {
            _habitRepository.DeleteHabitCompletionById(habitCompletionId);
        }

        public void DeleteDailyHabitCompletion(int dailyHabitCompletionId)
        {
            _dailyHabitRepository.DeleteDailyHabitCompletionById(dailyHabitCompletionId);
        }
    }
}
