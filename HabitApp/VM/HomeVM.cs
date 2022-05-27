using HabitApp.Data;
using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class HomeVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;
        private readonly AllHabitService _allHabitService;

        public HomeVM(PageNavigationManager pageNavigationManager, AllHabitService allHabitService)
        {
            _pageNavigationManager = pageNavigationManager;
            _allHabitService = allHabitService;

            ChangeHabitCommand = new BaseCommand(OnChangeHabitCommandExecuted, CanChangeHabitCommandExecute);
            ChangeDailyHabitCommand = new BaseCommand(OnChangeDailyHabitCommandExecuted, CanChangeDailyHabitCommandExecute);
            ChangeTaskCommand = new BaseCommand(OnChangeTaskCommandExecuted, CanChangeTaskCommandExecute);
        }

        #region Habits : List<Habit> - Список привычек пользователя

        /// <summary>Список привычек пользователя</summary>
        private List<Habit> _Habits;

        /// <summary>Список привычек пользователя</summary>
        public List<Habit> Habits
        {
            get => _Habits;
            set => Set(ref _Habits, value);
        }

        #endregion

        #region DailyHabits : List<DailyHabit> - Список ежедневных привычек

        /// <summary>Список ежедневных привычек</summary>
        private List<DailyHabit> _DailyHabits;

        /// <summary>Список ежедневных привычек</summary>
        public List<DailyHabit> DailyHabits
        {
            get => _DailyHabits;
            set => Set(ref _DailyHabits, value);
        }

        #endregion

        #region Tasks : List<Task> - Список задач пользователя

        /// <summary>Список задач пользователя</summary>
        private List<Task> _Tasks;

        /// <summary>Список задач пользователя</summary>
        public List<Task> Tasks
        {
            get => _Tasks;
            set => Set(ref _Tasks, value);
        }

        #endregion


        #region SelectedHabitIndex : int? - Индекс выбранной привычки в списке

        /// <summary>Индекс выбранной привычки в списке</summary>
        private int? _SelectedHabitIndex;

        /// <summary>Индекс выбранной привычки в списке</summary>
        public int? SelectedHabitIndex
        {
            get => _SelectedHabitIndex;
            set => Set(ref _SelectedHabitIndex, value);
        }

        #endregion

        #region SelectedDailyHabitIndex : int? - Индекс выбранной ежедневной привычки в списке

        /// <summary>Индекс выбранной ежедневной привычки в списке</summary>
        private int? _SelectedDailyHabitIndex;

        /// <summary>Индекс выбранной ежедневной привычки в списке</summary>
        public int? SelectedDailyHabitIndex
        {
            get => _SelectedDailyHabitIndex;
            set => Set(ref _SelectedDailyHabitIndex, value);
        }

        #endregion

        #region SelectedTaskIndex : int? - Индекс выбранной задачи в списке

        /// <summary>Индекс выбранной задачи в списке</summary>
        private int? _SelectedTaskIndex;

        /// <summary>Индекс выбранной задачи в списке</summary>
        public int? SelectedTaskIndex
        {
            get => _SelectedTaskIndex;
            set => Set(ref _SelectedTaskIndex, value);
        }

        #endregion

        // TODO: в привязке учесть, что эти значения могут быть null, нужно для обработки подобного установить TargetNullValue = ''

        #region ChangeHabitCommand

        public ICommand ChangeHabitCommand { get; }
        private bool CanChangeHabitCommandExecute(object p) => SelectedHabitIndex != null;

        private void OnChangeHabitCommandExecuted(object p)
        {
            Habits[SelectedHabitIndex.Value] = _allHabitService.ChangeHabit(Habits[SelectedHabitIndex.Value]);
        }

        #endregion

        #region ChangeDailyHabitCommand

        public ICommand ChangeDailyHabitCommand { get; }
        private bool CanChangeDailyHabitCommandExecute(object p) => SelectedDailyHabitIndex != null;

        private void OnChangeDailyHabitCommandExecuted(object p)
        {
            DailyHabits[SelectedDailyHabitIndex.Value] = _allHabitService.ChangeDailyHabit(DailyHabits[SelectedDailyHabitIndex.Value]);
        }

        #endregion

        #region ChangeTaskCommand

        public ICommand ChangeTaskCommand { get; }
        private bool CanChangeTaskCommandExecute(object p) => SelectedTaskIndex != null;

        private void OnChangeTaskCommandExecuted(object p)
        {
            Tasks[SelectedTaskIndex.Value] = _allHabitService.ChangeTask(Tasks[SelectedTaskIndex.Value]);
        }

        #endregion
    }
}
