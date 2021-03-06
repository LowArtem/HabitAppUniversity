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
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public HomeVM(PageNavigationManager pageNavigationManager, AllHabitCRUDService allHabitCRUDService, AllHabitService allHabitService, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _pageNavigationManager = pageNavigationManager;
            _allHabitCRUDService = allHabitCRUDService;
            _currentDateTimeProvider = currentDateTimeProvider;

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

            DeleteHabitCompletionCommand = new BaseCommand(OnDeleteHabitCompletionCommandExecuted, CanDeleteHabitCompletionCommandExecute);
            DeleteDailyHabitCompletionCommand = new BaseCommand(OnDeleteDailyHabitCompletionCommandExecuted, CanDeleteDailyHabitCompletionCommandExecute);

            ExitFromApplicationCommand = new BaseCommand(OnExitFromApplicationCommandExecuted, CanExitFromApplicationCommandExecute);
            LogoutCommand = new BaseCommand(OnLogoutCommandExecuted, CanLogoutCommandExecute);
            NavigateToDashboardCommand = new BaseCommand(OnNavigateToDashboardCommandExecuted, CanNavigateToDashboardCommandExecute);

            AddNewHabitCommand = new BaseCommand(OnAddNewHabitCommandExecuted, CanAddNewHabitCommandExecute);
            AddNewDailyHabitCommand = new BaseCommand(OnAddNewDailyHabitCommandExecuted, CanAddNewDailyHabitCommandExecute);
            AddNewTaskCommand = new BaseCommand(OnAddNewTaskCommandExecuted, CanAddNewTaskCommandExecute);

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

            Habits = _allHabitCRUDService.GetAllHabitsByUser(currentUser.Id);
            Habits.Sort((x, y) => x.Id.CompareTo(y.Id));

            DailyHabits = _allHabitCRUDService.GetAllDailyHabitsByUser(currentUser.Id);
            DailyHabits.Sort((x, y) => x.Id.CompareTo(y.Id));

            Tasks = _allHabitCRUDService.GetAllTasksByUser(currentUser.Id);
            Tasks.Sort((x, y) => x.Id.CompareTo(y.Id));

            CategoriesList = Categories.GetAll();
            _allHabitService = allHabitService;

            IsSelectedDailyHabitNotNull = SelectedDailyHabit != null;
            IsSelectedHabitNotNull = SelectedHabit != null;
            IsSelectedTaskNotNull = SelectedTask != null;

            UpdateDailyHabitsStatus();

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
        }


        /// <summary>
        /// Обновление DailyHabits.Status если произошла смена дня
        /// </summary>
        private void UpdateDailyHabitsStatus()
        {
            var currentDateTime = _currentDateTimeProvider.GetCurrentDateTime();
            var savedDateTime = Properties.Settings.Default.lastLoginTime;

            Properties.Settings.Default.lastLoginTime = currentDateTime;

            // сменилась дата, значит нужно обновлять статус ежедневных привычек
            if (savedDateTime.Date < currentDateTime.Date)
            {
                foreach (var dailyHabit in DailyHabits)
                {
                    _allHabitCRUDService.ChangeDailyHabitStatus(false, dailyHabit.Id);
                }

                OnPropertyChanged(nameof(DailyHabits));
            }
        }

        #region Username : string - Имя текущего пользователя

        /// <summary>Имя текущего пользователя</summary>
        private string _Username = (Application.Current as App).CurrentUser.Username;

        /// <summary>Имя текущего пользователя</summary>
        public string Username
        {
            get => _Username;
        }

        #endregion



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
            else
            {
                SelectedDailyHabitIndex = index;

                var lastCompletion = DailyHabitCompletions
                    .FindAll(x => x.Date.Date == _currentDateTimeProvider.GetCurrentDateTime().Date)
                    .Aggregate((curMin, x) => x.Date > curMin.Date ? x : curMin);

                DeleteDailyHabitCompletionCommand.Execute(lastCompletion.Id);
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

            _allHabitService.ChangeTaskCompletedStatus(
                Tasks[index].Id, (Application.Current as App).CurrentUser.Id, 
                Tasks[index].Status, _currentDateTimeProvider.GetCurrentDateTime());

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


        #region AddNewHabitCommand

        public ICommand AddNewHabitCommand { get; }
        private bool CanAddNewHabitCommandExecute(object p) => true;

        private void OnAddNewHabitCommandExecuted(object p)
        {
            if (!IsNewHabitCreating)
            {
                SelectedHabitIndex = null;
                SelectedHabit = null;
                HabitCompletions = null;

                SelectedHabit = new Habit("", "", Categories.Other, (Application.Current as App).CurrentUser.Id);

                IsNewHabitCreating = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedHabit.Name) &&
                    !string.IsNullOrEmpty(SelectedHabit.Description))
                {
                    var newHabit = _allHabitCRUDService.AddNewHabit(SelectedHabit);
                    Habits.Add(newHabit);

                    OnPropertyChanged(nameof(Habits));
                }
                else
                {
                    MessageQueue.Enqueue("You have to fill all fields to create new habit");
                }

                IsNewHabitCreating = false;
            }
        }

        #endregion

        #region IsNewHabitCreating : bool - Пользователь сейчас создаёт новую привычку

        /// <summary>Пользователь сейчас создаёт новую привычку</summary>
        private bool _IsNewHabitCreating = false;

        /// <summary>Пользователь сейчас создаёт новую привычку</summary>
        public bool IsNewHabitCreating
        {
            get => _IsNewHabitCreating;
            set => Set(ref _IsNewHabitCreating, value);
        }

        #endregion

        #region IsSelectedHabitNotNull : bool - Ни одна привычка не выбрана

        /// <summary>Ни одна привычка не выбрана</summary>
        private bool _IsSelectedHabitNotNull;

        /// <summary>Ни одна привычка не выбрана</summary>
        public bool IsSelectedHabitNotNull
        {
            get => _IsSelectedHabitNotNull;
            set => Set(ref _IsSelectedHabitNotNull, value);
        }

        #endregion




        #region AddNewDailyHabitCommand

        public ICommand AddNewDailyHabitCommand { get; }
        private bool CanAddNewDailyHabitCommandExecute(object p) => true;

        private void OnAddNewDailyHabitCommandExecuted(object p)
        {
            if (!IsNewDailyHabitCreating)
            {
                SelectedDailyHabitIndex = null;
                SelectedDailyHabit = null;
                DailyHabitCompletions = null;

                SelectedDailyHabit = new DailyHabit("", "", Categories.Other, 
                    (Application.Current as App).CurrentUser.Id, 
                    new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0));

                IsNewDailyHabitCreating = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedDailyHabit.Name) &&
                    !string.IsNullOrEmpty(SelectedDailyHabit.Description))
                {
                    var newHabit = _allHabitCRUDService.AddNewDailyHabit(SelectedDailyHabit);
                    DailyHabits.Add(newHabit);

                    OnPropertyChanged(nameof(DailyHabits));
                }
                else
                {
                    MessageQueue.Enqueue("You have to fill all fields to create new daily habit");
                }

                IsNewDailyHabitCreating = false;
            }
        }

        #endregion

        #region IsNewDailyHabitCreating : bool - Пользователь сейчас создаёт новую ежедневную привычку

        /// <summary>Пользователь сейчас создаёт новую ежедневную привычку</summary>
        private bool _IsNewDailyHabitCreating = false;

        /// <summary>Пользователь сейчас создаёт новую ежедневную привычку</summary>
        public bool IsNewDailyHabitCreating
        {
            get => _IsNewDailyHabitCreating;
            set => Set(ref _IsNewDailyHabitCreating, value);
        }

        #endregion

        #region IsSelectedDailyHabitNotNull : bool - Ни одна ежедневная привычка не выбрана

        /// <summary>Ни одна ежедневная привычка не выбрана</summary>
        private bool _IsSelectedDailyHabitNotNull;

        /// <summary>Ни одна ежедневная привычка не выбрана</summary>
        public bool IsSelectedDailyHabitNotNull
        {
            get => _IsSelectedDailyHabitNotNull;
            set => Set(ref _IsSelectedDailyHabitNotNull, value);
        }

        #endregion




        #region AddNewTaskCommand

        public ICommand AddNewTaskCommand { get; }
        private bool CanAddNewTaskCommandExecute(object p) => true;

        private void OnAddNewTaskCommandExecuted(object p)
        {
            if (!IsNewTaskCreating)
            {
                SelectedTaskIndex = null;
                SelectedTask = null;

                SelectedTask = new Task("", "", new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0));

                IsNewTaskCreating = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedTask.Name) &&
                    !string.IsNullOrEmpty(SelectedTask.Description))
                {
                    var newHabit = _allHabitCRUDService.AddNewTask(SelectedTask, (Application.Current as App).CurrentUser.Id);
                    Tasks.Add(newHabit);

                    OnPropertyChanged(nameof(Tasks));
                }
                else
                {
                    MessageQueue.Enqueue("You have to fill all fields to create task");
                }

                IsNewTaskCreating = false;
            }
        }

        #endregion

        #region IsNewTaskCreating : bool - Пользователь сейччас создаёт новую задачу

        /// <summary>Пользователь сейччас создаёт новую задачу</summary>
        private bool _IsNewTaskCreating = false;

        /// <summary>Пользователь сейччас создаёт новую задачу</summary>
        public bool IsNewTaskCreating
        {
            get => _IsNewTaskCreating;
            set => Set(ref _IsNewTaskCreating, value);
        }

        #endregion

        #region IsSelectedTaskNotNull : bool - Ни одна задача не выбрана

        /// <summary>Ни одна задача не выбрана</summary>
        private bool _IsSelectedTaskNotNull;

        /// <summary>Ни одна задача не выбрана</summary>
        public bool IsSelectedTaskNotNull
        {
            get => _IsSelectedTaskNotNull;
            set => Set(ref _IsSelectedTaskNotNull, value);
        }

        #endregion





        #region DeleteHabitCompletionCommand

        public ICommand DeleteHabitCompletionCommand { get; }
        private bool CanDeleteHabitCompletionCommandExecute(object p) => true;

        private void OnDeleteHabitCompletionCommandExecuted(object p)
        {
            int index = HabitCompletions.FindIndex(x => x.Id == int.Parse(p.ToString()));
            HabitCompletions.Remove(HabitCompletions[index]);
            _allHabitService.DeleteHabitCompletion(int.Parse(p.ToString()));

            OnPropertyChanged(nameof(HabitCompletions));
        }

        #endregion

        #region DeleteDailyHabitCompletionCommand

        public ICommand DeleteDailyHabitCompletionCommand { get; }
        private bool CanDeleteDailyHabitCompletionCommandExecute(object p) => true;

        private void OnDeleteDailyHabitCompletionCommandExecuted(object p)
        {
            int index = DailyHabitCompletions.FindIndex(x => x.Id == int.Parse(p.ToString()));
            DailyHabitCompletions.Remove(DailyHabitCompletions[index]);
            _allHabitService.DeleteDailyHabitCompletion(int.Parse(p.ToString()));

            OnPropertyChanged(nameof(DailyHabitCompletions));
        }

        #endregion



        #region ExitFromApplicationCommand

        public ICommand ExitFromApplicationCommand { get; }
        private bool CanExitFromApplicationCommandExecute(object p) => true;

        private void OnExitFromApplicationCommandExecuted(object p)
        {
            App.Current.Shutdown();
        }

        #endregion

        #region LogoutCommand

        public ICommand LogoutCommand { get; }
        private bool CanLogoutCommandExecute(object p) => true;

        private void OnLogoutCommandExecuted(object p)
        {
            Properties.Settings.Default.userId = -1;
            Properties.Settings.Default.Save();

            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<LoginView>());
        }

        #endregion

        #region NavigateToDashboardCommand

        public ICommand NavigateToDashboardCommand { get; }
        private bool CanNavigateToDashboardCommandExecute(object p) => true;

        private void OnNavigateToDashboardCommandExecuted(object p)
        {
            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<DashboardView>());
        }

        #endregion


        #region MessageQueue : SnackbarMessageQueue - Сообщение об ошибке

        /// <summary>Сообщение об ошибке</summary>
        private SnackbarMessageQueue _MessageQueue;

        /// <summary>Сообщение об ошибке</summary>
        public SnackbarMessageQueue MessageQueue
        {
            get => _MessageQueue;
            set => Set(ref _MessageQueue, value);
        }

        #endregion



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            IsSelectedDailyHabitNotNull = SelectedDailyHabit != null;
            IsSelectedHabitNotNull = SelectedHabit != null;
            IsSelectedTaskNotNull = SelectedTask != null;

            if (propertyName == nameof(Habits))
            {
                IsHabitsEmpty = Habits.Count == 0;
            }
            else if (propertyName == nameof(DailyHabits))
            {
                IsDailyHabitsEmpty = DailyHabits.Count == 0;
            }
            else if (propertyName == nameof(Tasks))
            {
                IsTasksEmpty = Tasks.Count == 0;
            }
            else if (propertyName == nameof(SelectedTabIndex))
            {
                IsHabitDetailedShowed = SelectedTabIndex == 0;
                IsDailyHabitDetailedShowed = SelectedTabIndex == 1;
                IsTaskDetailedShowed = SelectedTabIndex == 2;
            }
            else if (propertyName == nameof(SelectedHabitIndex))
            {
                if (SelectedHabitIndex.HasValue && SelectedHabitIndex.Value >= 0)
                {
                    SelectedHabit = (Habit)Habits[SelectedHabitIndex.Value].Clone();
                    HabitCompletions = _allHabitService.GetHabitCompletions(SelectedHabit.Id);

                    HabitCompletionsPositive.Clear();
                    HabitCompletionsNegative.Clear();

                    HabitCompletionsPositive.AddRange(HabitCompletions.FindAll(x => x.IsPositive));
                    HabitCompletionsNegative.AddRange(HabitCompletions.FindAll(x => !x.IsPositive));

                    OnPropertyChanged(nameof(HabitCompletionsPositive));
                    OnPropertyChanged(nameof(HabitCompletionsNegative));
                }
                else
                {
                    SelectedHabit = null;
                }
            }
            else if (propertyName == nameof(SelectedDailyHabitIndex))
            {
                if (SelectedDailyHabitIndex.HasValue && SelectedDailyHabitIndex.Value >= 0)
                {
                    SelectedDailyHabit = (DailyHabit)DailyHabits[SelectedDailyHabitIndex.Value].Clone();
                    DailyHabitCompletions = _allHabitService.GetDailyHabitCompletions(SelectedDailyHabit.Id);
                }
                else
                {
                    SelectedDailyHabit = null;
                }
            }
            else if (propertyName == nameof(SelectedTaskIndex))
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
