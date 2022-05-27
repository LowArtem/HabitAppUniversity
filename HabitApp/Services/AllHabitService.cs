using HabitApp.Data;
using HabitApp.Implementation;
using System;
using System.Collections.Generic;

namespace HabitApp.Services
{
    public class AllHabitService
    {
        private readonly HabitRepository _habitRepository;
        private readonly DailyHabitRepository _dailyHabitRepository;
        private readonly TaskRepository _taskRepository;

        public AllHabitService(HabitRepository habitRepository, DailyHabitRepository dailyHabitRepository, TaskRepository taskRepository)
        {
            _habitRepository = habitRepository;
            _dailyHabitRepository = dailyHabitRepository;
            _taskRepository = taskRepository;
        }

        #region GetAll_ByUser
        public List<Habit> GetAllHabitsByUser(int userid)
        {
            return _habitRepository.GetAllByUserId(userid);
        }

        public List<DailyHabit> GetAllDailyHabitsByUser(int userid)
        {
            return _dailyHabitRepository.GetAllByUserId(userid);
        }

        public List<Data.Task> GetAllTasksByUser(int userid)
        {
            return _taskRepository.GetAllTasksByUser(userid);
        }
        #endregion

        #region AddNew_
        public Habit AddNewHabit(Habit habit)
        {
            return _habitRepository.Add(habit);
        }

        public DailyHabit AddNewDailyHabit(DailyHabit dailyHabit)
        {
            return _dailyHabitRepository.Add(dailyHabit);
        }

        public Data.Task AddNewTask(Data.Task task)
        {
            return _taskRepository.Add(task);
        }
        #endregion

        #region Change_

        #region ChangeHabit
        public Habit ChangeHabitName(string name, int habitid)
        {
            var habit = _habitRepository.GetById(habitid);
            habit.Name = name;
            return _habitRepository.Update(habit);
        }

        public Habit ChangeHabitDescription(string description, int habitid)
        {
            var habit = _habitRepository.GetById(habitid);
            habit.Description = description;
            return _habitRepository.Update(habit);
        }

        public Habit ChangeHabitDifficulty(int difficulty, int habitid)
        {
            var habit = _habitRepository.GetById(habitid);
            habit.Difficulty = difficulty;
            return _habitRepository.Update(habit);
        }
        #endregion

        #region ChangeDailyHabit
        public DailyHabit ChangeDailyHabitName(string name, int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Name = name;
            return _dailyHabitRepository.Update(dailyHabit);
        }

        public DailyHabit ChangeDailyHabitDescription(string description, int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Description = description;
            return _dailyHabitRepository.Update(dailyHabit);
        }

        public DailyHabit ChangeDailyHabitStatus(bool status, int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Status = status;
            return _dailyHabitRepository.Update(dailyHabit);
        }

        public DailyHabit ChangeDailyHabitDifficulty(int difficulty, int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Difficulty = difficulty;
            return _dailyHabitRepository.Update(dailyHabit);
        }

        public DailyHabit ChangeDailyHabitDeadline(DateTime deadline, int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            dailyHabit.Deadline = deadline;
            return _dailyHabitRepository.Update(dailyHabit);
        }
        #endregion

        #region ChangeTask
        public Data.Task ChangeTaskName(string name, int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Name = name;
            return _taskRepository.Update(task);
        }

        public Data.Task ChangeTaskDescription(string description, int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Description = description;
            return _taskRepository.Update(task);
        }

        public Data.Task ChangeTaskPriority(int priority, int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Priority = priority;
            return _taskRepository.Update(task);
        }

        public Data.Task ChangeTaskStatus(bool status, int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Status = status;
            return _taskRepository.Update(task);
        }

        public Data.Task ChangeTaskDeadline(DateTime deadline, int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            task.Deadline = deadline;
            return _taskRepository.Update(task);
        }
        #endregion

        #endregion

        #region Delete_
        public void DeleteHabit(int habitId)
        {
            var habit = _habitRepository.GetById(habitId);
            _habitRepository.Delete(habit);
        }

        public void DeleteDailyHabit(int dailyHabitId)
        {
            var dailyHabit = _dailyHabitRepository.GetById(dailyHabitId);
            _dailyHabitRepository.Delete(dailyHabit);
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            _taskRepository.Delete(task);
        }
        #endregion
    }
}
