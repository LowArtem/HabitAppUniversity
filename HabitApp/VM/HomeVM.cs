using HabitApp.Data;
using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class HomeVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;
        private readonly AllHabitCRUDService _allHabitCRUDService;
        private readonly AllHabitService _allHabitService;

        public HomeVM(PageNavigationManager pageNavigationManager, AllHabitCRUDService allHabitCRUDService, AllHabitService allHabitService)
        {
            _pageNavigationManager = pageNavigationManager;
            _allHabitCRUDService = allHabitCRUDService;

            ChangeHabitCommand = new BaseCommand(OnChangeHabitCommandExecuted, CanChangeHabitCommandExecute);
            CancelHabitChangingCommand = new BaseCommand(OnCancelHabitChangingCommandExecuted, CanCancelHabitChangingCommandExecute);

            ChangeDailyHabitCommand = new BaseCommand(OnChangeDailyHabitCommandExecuted, CanChangeDailyHabitCommandExecute);
            ChangeDailyHabitStatusCommand = new BaseCommand(OnChangeDailyHabitStatusCommandExecuted, CanChangeDailyHabitStatusCommandExecute);
            CancelDailyHabitChangingCommand = new BaseCommand(OnCancelDailyHabitChangingCommandExecuted, CanCancelDailyHabitChangingCommandExecute);

            ChangeTaskCommand = new BaseCommand(OnChangeTaskCommandExecuted, CanChangeTaskCommandExecute);
            ChangeTaskStatusCommand = new BaseCommand(OnChangeTaskStatusCommandExecuted, CanChangeTaskStatusCommandExecute);
            CancelTaskChangingCommand = new BaseCommand(OnCancelTaskChangingCommandExecuted, CanCancelTaskChangingCommandExecute);

            AddHabitCompletionPositiveCommand = new BaseCommand(OnAddHabitCompletionPositiveCommandExecutedAsync, CanAddHabitCompletionPositiveCommandExecute);
            AddHabitCompletionNegativeCommand = new BaseCommand(OnAddHabitCompletionNegativeCommandExecutedAsync, CanAddHabitCompletionNegativeCommandExecute);

            User currentUser;
            var app = Application.Current;
            if (app is App currentApp)
            {
                currentUser = currentApp.CurrentUser;
            }
            else
            {
                throw new Exception("Неверный класс приложения, не обнаружено свойство CurrentUser");
            }

            // TODO: написать сложный сравниватель, учитывающий приоритет, дедлайн и т д

            Habits = _allHabitCRUDService.GetAllHabitsByUser(currentUser.Id);
            Habits.Sort((x, y) => x.Id.CompareTo(y.Id));

            DailyHabits = _allHabitCRUDService.GetAllDailyHabitsByUser(currentUser.Id);
            DailyHabits.Sort((x, y) => x.Id.CompareTo(y.Id));

            Tasks = _allHabitCRUDService.GetAllTasksByUser(currentUser.Id);
            Tasks.Sort((x, y) => x.Id.CompareTo(y.Id));

            CategoriesList = Categories.GetAll();
            _allHabitService = allHabitService;
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

        #region IsHabitsEmpty : bool - Пустой ли список привычек

        /// <summary>Пустой ли список привычек</summary>
        private bool _IsHabitsEmpty = true;

        /// <summary>Пустой ли список привычек</summary>
        public bool IsHabitsEmpty
        {
            get => _IsHabitsEmpty;
            set => Set(ref _IsHabitsEmpty, value);
        }

        #endregion



        #region HabitCompletions : List<HabitCompletion> - список всех выполнений выбранной привычки

        /// <summary>список всех выполнений выбранной привычки</summary>
        private List<HabitCompletion> _HabitCompletions = new List<HabitCompletion>();

        /// <summary>список всех выполнений выбранной привычки</summary>
        public List<HabitCompletion> HabitCompletions
        {
            get => _HabitCompletions;
            set => Set(ref _HabitCompletions, value);
        }

        #endregion

        #region HabitCompletionsPositive : List<HabitCompletion> - список положительных выполнений выбранной привычки

        /// <summary>список положительных выполнений выбранной привычки</summary>
        private List<HabitCompletion> _HabitCompletionsPositive = new List<HabitCompletion>();

        /// <summary>список положительных выполнений выбранной привычки</summary>
        public List<HabitCompletion> HabitCompletionsPositive
        {
            get => _HabitCompletionsPositive;
            set => Set(ref _HabitCompletionsPositive, value);
        }

        #endregion

        #region HabitCompletionsNegative : List<HabitCompletion> - список отрицательных выполнений выбранной привычки

        /// <summary>список отрицательных выполнений выбранной привычки</summary>
        private List<HabitCompletion> _HabitCompletionsNegative = new List<HabitCompletion>();

        /// <summary>список отрицательных выполнений выбранной привычки</summary>
        public List<HabitCompletion> HabitCompletionsNegative
        {
            get => _HabitCompletionsNegative;
            set => Set(ref _HabitCompletionsNegative, value);
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

        #region IsDailyHabitsEmpty : bool - Пустой ли список ежедневных привычек

        /// <summary>Пустой ли список ежедневных привычек</summary>
        private bool _IsDailyHabitsEmpty = true;

        /// <summary>Пустой ли список ежедневных привычек</summary>
        public bool IsDailyHabitsEmpty
        {
            get => _IsDailyHabitsEmpty;
            set => Set(ref _IsDailyHabitsEmpty, value);
        }

        #endregion


        #region DailyHabitCompletions : List<HabitCompletion> - список всех выполнений выбранной ежедневной привычки

        /// <summary>список всех выполнений выбранной ежедневной привычки</summary>
        private List<HabitCompletion> _DailyHabitCompletions = new List<HabitCompletion>();

        /// <summary>список всех выполнений выбранной ежедневной привычки</summary>
        public List<HabitCompletion> DailyHabitCompletions
        {
            get => _DailyHabitCompletions;
            set => Set(ref _DailyHabitCompletions, value);
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

        #region IsTasksEmpty : bool - Пустой ли список задач

        /// <summary>Пустой ли список задач</summary>
        private bool _IsTasksEmpty = true;

        /// <summary>Пустой ли список задач</summary>
        public bool IsTasksEmpty
        {
            get => _IsTasksEmpty;
            set => Set(ref _IsTasksEmpty, value);
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

        #region SelectedHabit : Habit - Выбранная привычка

        /// <summary>Выбранная привычка</summary>
        private Habit _SelectedHabit;

        /// <summary>Выбранная привычка</summary>
        public Habit SelectedHabit
        {
            get => _SelectedHabit;
            set => Set(ref _SelectedHabit, value);
        }

        #endregion


        #region CategoriesList : List<string> - Список всех возможных категорий

        /// <summary>Список всех возможных категорий</summary>
        private List<string> _CategoriesList;

        /// <summary>Список всех возможных категорий</summary>
        public List<string> CategoriesList
        {
            get => _CategoriesList;
            set => Set(ref _CategoriesList, value);
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

        #region SelectedDailyHabit : DailyHabit - Выбранная ежедневная привычка

        /// <summary>Выбранная ежедневная привычка</summary>
        private DailyHabit _SelectedDailyHabit;

        /// <summary>Выбранная ежедневная привычка</summary>
        public DailyHabit SelectedDailyHabit
        {
            get => _SelectedDailyHabit;
            set => Set(ref _SelectedDailyHabit, value);
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

        #region SelectedTask : Task - Выбранная задача

        /// <summary>Выбранная задача</summary>
        private Task _SelectedTask;

        /// <summary>Выбранная задача</summary>
        public Task SelectedTask
        {
            get => _SelectedTask;
            set => Set(ref _SelectedTask, value);
        }

        #endregion



        #region SelectedTabIndex : int - Выбранный набор данных (1-habits, 2-daily, 3-tasks)

        /// <summary>Выбранный набор данных (1-habits, 2-daily, 3-tasks)</summary>
        private int _SelectedTabIndex;

        /// <summary>Выбранный набор данных (1-habits, 2-daily, 3-tasks)</summary>
        public int SelectedTabIndex
        {
            get => _SelectedTabIndex;
            set => Set(ref _SelectedTabIndex, value);
        }

        #endregion

        #region IsHabitDetailedShowed : bool - Показывать детали привычки

        /// <summary>Показывать детали привычки</summary>
        private bool _IsHabitDetailedShowed = true;

        /// <summary>Показывать детали привычки</summary>
        public bool IsHabitDetailedShowed
        {
            get => _IsHabitDetailedShowed;
            set => Set(ref _IsHabitDetailedShowed, value);
        }

        #endregion

        #region IsDailyHabitDetailedShowed : bool - Показывать детали ежедневной привычки

        /// <summary>Показывать детали ежедневной привычки</summary>
        private bool _IsDailyHabitDetailedShowed = false;

        /// <summary>Показывать детали ежедневной привычки</summary>
        public bool IsDailyHabitDetailedShowed
        {
            get => _IsDailyHabitDetailedShowed;
            set => Set(ref _IsDailyHabitDetailedShowed, value);
        }

        #endregion

        #region IsTaskDetailedShowed : bool - Показывать детали задачи

        /// <summary>Показывать детали задачи</summary>
        private bool _IsTaskDetailedShowed = false;

        /// <summary>Показывать детали задачи</summary>
        public bool IsTaskDetailedShowed
        {
            get => _IsTaskDetailedShowed;
            set => Set(ref _IsTaskDetailedShowed, value);
        }

        #endregion



        #region ChangeHabitCommand

        public ICommand ChangeHabitCommand { get; }
        private bool CanChangeHabitCommandExecute(object p) => SelectedHabitIndex != null;

        private void OnChangeHabitCommandExecuted(object p)
        {            
            Habits[SelectedHabitIndex.Value].Name = SelectedHabit.Name;
            Habits[SelectedHabitIndex.Value].Description = SelectedHabit.Description;
            Habits[SelectedHabitIndex.Value].Category = SelectedHabit.Category;
            Habits[SelectedHabitIndex.Value].Difficulty = SelectedHabit.Difficulty;
            Habits[SelectedHabitIndex.Value].Type = SelectedHabit.Type;
            Habits[SelectedHabitIndex.Value] = _allHabitCRUDService.ChangeHabit(Habits[SelectedHabitIndex.Value]);

            OnPropertyChanged(nameof(Habits));
        }

        #endregion

        #region CancelHabitChangingCommand

        public ICommand CancelHabitChangingCommand { get; }
        private bool CanCancelHabitChangingCommandExecute(object p) => SelectedHabitIndex != null;

        private void OnCancelHabitChangingCommandExecuted(object p)
        {
            var temp = SelectedHabitIndex;
            SelectedHabitIndex = null;
            SelectedHabitIndex = temp;
        }

        #endregion


        #region ChangeDailyHabitCommand

        public ICommand ChangeDailyHabitCommand { get; }
        private bool CanChangeDailyHabitCommandExecute(object p) => SelectedDailyHabitIndex != null;

        private void OnChangeDailyHabitCommandExecuted(object p)
        {
            DailyHabits[SelectedDailyHabitIndex.Value] = SelectedDailyHabit;
            DailyHabits[SelectedDailyHabitIndex.Value] = _allHabitCRUDService.ChangeDailyHabit(DailyHabits[SelectedDailyHabitIndex.Value]);

            OnPropertyChanged(nameof(DailyHabits));
        }

        #endregion

        #region ChangeDailyHabitStatusCommand

        public ICommand ChangeDailyHabitStatusCommand { get; }
        private bool CanChangeDailyHabitStatusCommandExecute(object p) => true;

        private async void OnChangeDailyHabitStatusCommandExecuted(object p)
        {
            int index = DailyHabits.FindIndex(x => x.Id == int.Parse(p.ToString()));

            // Dialog games
            if (DailyHabits[index].Status == true)
            {
                var view = App.Host.Services.GetRequiredService<CompletionRatingDialog>();
                var result = await DialogHost.Show(view, "Root Dialog", ClosingEventHandler);

                var rating = int.Parse((result ?? null).ToString());
                if (rating > 0)
                {
                    _allHabitService.AddDailyHabitCompletion(DailyHabits[index].Id, rating);
                    DailyHabits[index].Status = true;
                }
                else
                {
                    DailyHabits[index].Status = false;
                }    
            }

            DailyHabits[index] = _allHabitCRUDService.ChangeDailyHabit(DailyHabits[index]);
            OnPropertyChanged(nameof(DailyHabits));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            return;
        }

        #endregion


        #region CancelDailyHabitChangingCommand

        public ICommand CancelDailyHabitChangingCommand { get; }
        private bool CanCancelDailyHabitChangingCommandExecute(object p) => SelectedDailyHabitIndex != null;

        private void OnCancelDailyHabitChangingCommandExecuted(object p)
        {
            var temp = SelectedDailyHabitIndex;
            SelectedDailyHabitIndex = null;
            SelectedDailyHabitIndex = temp;
        }

        #endregion


        #region ChangeTaskCommand

        public ICommand ChangeTaskCommand { get; }
        private bool CanChangeTaskCommandExecute(object p) => SelectedTaskIndex != null;

        private void OnChangeTaskCommandExecuted(object p)
        {
            Tasks[SelectedTaskIndex.Value] = SelectedTask;
            Tasks[SelectedTaskIndex.Value].Deadline = SelectedTask.Deadline;
            Tasks[SelectedTaskIndex.Value] = _allHabitCRUDService.ChangeTask(Tasks[SelectedTaskIndex.Value]);
            OnPropertyChanged(nameof(Tasks));
        }

        #endregion

        #region ChangeTaskStatusCommand

        public ICommand ChangeTaskStatusCommand { get; }
        private bool CanChangeTaskStatusCommandExecute(object p) => true;

        private void OnChangeTaskStatusCommandExecuted(object p)
        {
            int index = Tasks.FindIndex(x => x.Id == int.Parse(p.ToString()));
            Tasks[index] = _allHabitCRUDService.ChangeTask(Tasks[index]);
            OnPropertyChanged(nameof(Tasks));
        }

        #endregion


        #region CancelTaskChangingCommand

        public ICommand CancelTaskChangingCommand { get; }
        private bool CanCancelTaskChangingCommandExecute(object p) => SelectedTaskIndex != null;

        private void OnCancelTaskChangingCommandExecuted(object p)
        {
            var temp = SelectedTaskIndex;
            SelectedTaskIndex = null;
            SelectedTaskIndex = temp;
        }

        #endregion


        #region AddHabitCompletionPositiveCommand

        public ICommand AddHabitCompletionPositiveCommand { get; }
        private bool CanAddHabitCompletionPositiveCommandExecute(object p) 
            => SelectedHabit != null && (SelectedHabit.Type == 1 || SelectedHabit.Type == 3);

        private async void OnAddHabitCompletionPositiveCommandExecutedAsync(object p)
        {
            // Dialog games            
            var view = App.Host.Services.GetRequiredService<CompletionRatingDialog>();
            var result = await DialogHost.Show(view, "Root Dialog", ClosingEventHandler);

            var rating = int.Parse((result ?? null).ToString());
            if (rating > 0)
            {
                _allHabitService.AddHabitCompletion(SelectedHabit.Id, rating, true);
                OnPropertyChanged(nameof(SelectedHabitIndex));
            }
            else
            {
                return;
            }            
        }

        #endregion

        #region AddHabitCompletionNegativeCommand

        public ICommand AddHabitCompletionNegativeCommand { get; }
        private bool CanAddHabitCompletionNegativeCommandExecute(object p) 
            => SelectedHabit != null && (SelectedHabit.Type == 2 || SelectedHabit.Type == 3);

        private async void OnAddHabitCompletionNegativeCommandExecutedAsync(object p)
        {
            // Dialog games            
            var view = App.Host.Services.GetRequiredService<CompletionRatingDialog>();
            var result = await DialogHost.Show(view, "Root Dialog", ClosingEventHandler);

            var rating = int.Parse((result ?? null).ToString());
            if (rating > 0)
            {
                _allHabitService.AddHabitCompletion(SelectedHabit.Id, rating, false);
                OnPropertyChanged(nameof(SelectedHabitIndex));
            }
            else
            {
                return;
            }
        }

        #endregion



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Habits")
                IsHabitsEmpty = Habits.Count == 0;
            else if (propertyName == "DailyHabits")
                IsDailyHabitsEmpty = DailyHabits.Count == 0;
            else if (propertyName == "Tasks")
                IsTasksEmpty = Tasks.Count == 0;
            else if (propertyName == "SelectedTabIndex")
            {
                IsHabitDetailedShowed = SelectedTabIndex == 0;
                IsDailyHabitDetailedShowed = SelectedTabIndex == 1;
                IsTaskDetailedShowed = SelectedTabIndex == 2;
            }
            else if (propertyName == "SelectedHabitIndex")
            {
                if (SelectedHabitIndex.HasValue && SelectedHabitIndex.Value >= 0)
                {
                    SelectedHabit = (Habit)Habits[SelectedHabitIndex.Value].Clone();
                    HabitCompletions = _allHabitService.GetHabitCompletions(SelectedHabit.Id);

                    HabitCompletionsPositive.Clear();
                    HabitCompletionsNegative.Clear();

                    HabitCompletionsPositive.AddRange(HabitCompletions.FindAll(x => x.IsPositive));
                    HabitCompletionsNegative.AddRange(HabitCompletions.FindAll(x => !x.IsPositive));

                    HabitCompletionsPositive.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                    HabitCompletionsNegative.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                    OnPropertyChanged(nameof(HabitCompletionsPositive));
                    OnPropertyChanged(nameof(HabitCompletionsNegative));
                }
                else
                {
                    SelectedHabit = null;
                }
            }
            else if (propertyName == "SelectedDailyHabitIndex")
            {
                if (SelectedDailyHabitIndex.HasValue && SelectedDailyHabitIndex.Value >= 0)
                {
                    SelectedDailyHabit = (DailyHabit)DailyHabits[SelectedDailyHabitIndex.Value].Clone();
                    DailyHabitCompletions = _allHabitService.GetDailyHabitCompletions(SelectedDailyHabit.Id);

                    DailyHabitCompletions.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                    OnPropertyChanged(nameof(DailyHabitCompletions));
                }
                else
                {
                    SelectedDailyHabit = null;
                }
            }
            else if (propertyName == "SelectedTaskIndex")
            {
                if (SelectedTaskIndex.HasValue && SelectedTaskIndex.Value >= 0)
                {
                    SelectedTask = (Task)Tasks[SelectedTaskIndex.Value].Clone();
                }
                else
                {
                    SelectedTask = null;
                }
            }

            base.OnPropertyChanged(propertyName);
        }
    }
}
