using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class CountAndDate
    {
        public int Count { get; private set; }
        public DateTime Date { get; private set; }

        public CountAndDate(int count, DateTime date)
        {
            Count = count;
            Date = date;
        }
    }


    public class DashboardVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;
        private readonly StatisticsService _statisticsService;

        public DashboardVM(PageNavigationManager pageNavigationManager, StatisticsService statisticsService)
        {
            _pageNavigationManager = pageNavigationManager;
            _statisticsService = statisticsService;

            ExitFromApplicationCommand = new BaseCommand(OnExitFromApplicationCommandExecuted, CanExitFromApplicationCommandExecute);
            LogoutCommand = new BaseCommand(OnLogoutCommandExecuted, CanLogoutCommandExecute);
            NavigateToHomeViewCommand = new BaseCommand(OnNavigateToHomeViewCommandExecuted, CanNavigateToHomeViewCommandExecute);

            DrawDoneHabitsByPeriodsGraph();
        }

        private void DrawDoneHabitsByPeriodsGraph()
        {
            FillHabitsLists((Application.Current as App).CurrentUser.Id);

            ByDaySeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Habits done:",
                    Values = new ChartValues<int>(HabitsCountByDay.Select(x => x.Count))
                },
                new LineSeries
                {
                    Title = "Daily habits done:",
                    Values = new ChartValues<int>(DailyHabitsCountByDay.Select(x => x.Count))
                },
                new LineSeries
                {
                    Title = "Tasks done:",
                    Values = new ChartValues<int>(TasksCountByDay.Select(x => x.Count))
                }
            };
        }

        private void FillHabitsLists(int userId)
        {
            FillHabitsListsByPeriod(userId, StatisticsService.TypeOfPeriod.Day, HabitsCountByDay, DailyHabitsCountByDay, TasksCountByDay);
            FillHabitsListsByPeriod(userId, StatisticsService.TypeOfPeriod.Week, HabitsCountByWeek, DailyHabitsCountByWeek, TasksCountByWeek);
            FillHabitsListsByPeriod(userId, StatisticsService.TypeOfPeriod.Month, HabitsCountByMonth, DailyHabitsCountByMonth, TasksCountByMonth);
            FillHabitsListsByPeriod(userId, StatisticsService.TypeOfPeriod.Year, HabitsCountByYear, DailyHabitsCountByYear, TasksCountByYear);
        }

        private void FillHabitsListsByPeriod(
            int userId, 
            StatisticsService.TypeOfPeriod period,
            List<CountAndDate> habitsCollection,
            List<CountAndDate> dailyHabitsCollection,
            List<CountAndDate> tasksCollection)
        {
            DateTime firstDay = DateTime.UtcNow;

            switch (period)
            {
                case StatisticsService.TypeOfPeriod.Day: firstDay = DateTime.UtcNow.AddDays(-7);
                    break;
                case StatisticsService.TypeOfPeriod.Week: firstDay = DateTime.UtcNow.AddDays(-7 * 7);
                    break;
                case StatisticsService.TypeOfPeriod.Month: firstDay = DateTime.UtcNow.AddMonths(-12);
                    break;
                case StatisticsService.TypeOfPeriod.Year: firstDay = DateTime.UtcNow.AddYears(5);
                    break;
            }

            for (DateTime d = firstDay; d.Date <= DateTime.UtcNow.Date; d = ForCounter(d, period))
            {
                habitsCollection.Add(new CountAndDate(_statisticsService.GetHabitsCompletionsCountForPeriod(userId, period, d), d));
                dailyHabitsCollection.Add(new CountAndDate(_statisticsService.GetDailyHabitsCompletionsCountForPeriod(userId, period, d), d));
                tasksCollection.Add(new CountAndDate(_statisticsService.GetTasksCompletionsCountForPeriod(userId, period, d), d));
            }

            DateTime ForCounter(DateTime v, StatisticsService.TypeOfPeriod periodLocal)
            {
                switch (periodLocal)
                {
                    case StatisticsService.TypeOfPeriod.Day:
                        return v.AddDays(1);
                    case StatisticsService.TypeOfPeriod.Week:
                        return v.AddDays(7);
                    case StatisticsService.TypeOfPeriod.Month:
                        return v.AddMonths(1);
                    case StatisticsService.TypeOfPeriod.Year:
                        return v.AddYears(1);
                    default: throw new ArgumentException("Wrong period");
                }
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


        #region Counters

        #region HabitsCountByDay : List<CountAndDate> - Список количества завершённых привычек за день

        /// <summary>Список количества завершённых привычек за день</summary>
        private List<CountAndDate> _HabitsCountByDay = new List<CountAndDate>();

        /// <summary>Список количества завершённых привычек за день</summary>
        public List<CountAndDate> HabitsCountByDay
        {
            get => _HabitsCountByDay;
            set => Set(ref _HabitsCountByDay, value);
        }

        #endregion

        #region DailyHabitsCountByDay : List<CountAndDate> - Список количества завершённых ежедневных привычек за день

        /// <summary>Список количества завершённых ежедневных привычек за день</summary>
        private List<CountAndDate> _DailyHabitsCountByDay = new List<CountAndDate>();

        /// <summary>Список количества завершённых ежедневных привычек за день</summary>
        public List<CountAndDate> DailyHabitsCountByDay
        {
            get => _DailyHabitsCountByDay;
            set => Set(ref _DailyHabitsCountByDay, value);
        }

        #endregion

        #region TasksCountByDay : List<CountAndDate> - Список количества завершённых задач за день

        /// <summary>Список количества завершённых задач за день</summary>
        private List<CountAndDate> _TasksCountByDay = new List<CountAndDate>();

        /// <summary>Список количества завершённых задач за день</summary>
        public List<CountAndDate> TasksCountByDay
        {
            get => _TasksCountByDay;
            set => Set(ref _TasksCountByDay, value);
        }

        #endregion

        #region HabitsCountByWeek : List<CountAndDate> - Список количества завершённых привычек за неделю

        /// <summary>Список количества завершённых привычек за неделю</summary>
        private List<CountAndDate> _HabitsCountByWeek = new List<CountAndDate>();

        /// <summary>Список количества завершённых привычек за неделю</summary>
        public List<CountAndDate> HabitsCountByWeek
        {
            get => _HabitsCountByWeek;
            set => Set(ref _HabitsCountByWeek, value);
        }

        #endregion

        #region DailyHabitsCountByWeek : List<CountAndDate> - Список количества завершённых ежедневных привычек за неделю

        /// <summary>Список количества завершённых ежедневных привычек за неделю</summary>
        private List<CountAndDate> _DailyHabitsCountByWeek = new List<CountAndDate>();

        /// <summary>Список количества завершённых ежедневных привычек за неделю</summary>
        public List<CountAndDate> DailyHabitsCountByWeek
        {
            get => _DailyHabitsCountByWeek;
            set => Set(ref _DailyHabitsCountByWeek, value);
        }

        #endregion

        #region TasksCountByWeek : List<CountAndDate> - Список количества завершённых задач за неделю

        /// <summary>Список количества завершённых задач за неделю</summary>
        private List<CountAndDate> _TasksCountByWeek = new List<CountAndDate>();

        /// <summary>Список количества завершённых задач за неделю</summary>
        public List<CountAndDate> TasksCountByWeek
        {
            get => _TasksCountByWeek;
            set => Set(ref _TasksCountByWeek, value);
        }

        #endregion

        #region HabitsCountByMonth : List<CountAndDate> - Список количества завершённых привычек за месяц

        /// <summary>Список количества завершённых привычек за месяц</summary>
        private List<CountAndDate> _HabitsCountByMonth = new List<CountAndDate>();

        /// <summary>Список количества завершённых привычек за месяц</summary>
        public List<CountAndDate> HabitsCountByMonth
        {
            get => _HabitsCountByMonth;
            set => Set(ref _HabitsCountByMonth, value);
        }

        #endregion

        #region DailyHabitsCountByMonth : List<CountAndDate> - Список количества завершённых ежедневных привычек за месяц

        /// <summary>Список количества завершённых ежедневных привычек за месяц</summary>
        private List<CountAndDate> _DailyHabitsCountByMonth = new List<CountAndDate>();

        /// <summary>Список количества завершённых ежедневных привычек за месяц</summary>
        public List<CountAndDate> DailyHabitsCountByMonth
        {
            get => _DailyHabitsCountByMonth;
            set => Set(ref _DailyHabitsCountByMonth, value);
        }

        #endregion

        #region TasksCountByMonth : List<CountAndDate> - Список количества завершённых задач за месяц

        /// <summary>Список количества завершённых задач за месяц</summary>
        private List<CountAndDate> _TasksCountByMonth = new List<CountAndDate>();

        /// <summary>Список количества завершённых задач за месяц</summary>
        public List<CountAndDate> TasksCountByMonth
        {
            get => _TasksCountByMonth;
            set => Set(ref _TasksCountByMonth, value);
        }

        #endregion

        #region HabitsCountByYear : List<CountAndDate> - Список количества завершённых привычек за год

        /// <summary>Список количества завершённых привычек за год</summary>
        private List<CountAndDate> _HabitsCountByYear = new List<CountAndDate>();

        /// <summary>Список количества завершённых привычек за год</summary>
        public List<CountAndDate> HabitsCountByYear
        {
            get => _HabitsCountByYear;
            set => Set(ref _HabitsCountByYear, value);
        }

        #endregion

        #region DailyHabitsCountByYear : List<CountAndDate> - Список количества завершённых ежедневных задач за год

        /// <summary>Список количества завершённых ежедневных задач за год</summary>
        private List<CountAndDate> _DailyHabitsCountByYear = new List<CountAndDate>();

        /// <summary>Список количества завершённых ежедневных задач за год</summary>
        public List<CountAndDate> DailyHabitsCountByYear
        {
            get => _DailyHabitsCountByYear;
            set => Set(ref _DailyHabitsCountByYear, value);
        }

        #endregion

        #region TasksCountByYear : List<CountAndDate> - Список количества завершённых задач за год

        /// <summary>Список количества завершённых задач за год</summary>
        private List<CountAndDate> _TasksCountByYear = new List<CountAndDate>();

        /// <summary>Список количества завершённых задач за год</summary>
        public List<CountAndDate> TasksCountByYear
        {
            get => _TasksCountByYear;
            set => Set(ref _TasksCountByYear, value);
        }

        #endregion

        #endregion


        #region ByDaySeries : SeriesCollection - Коллекция точек ByDay

        /// <summary>Коллекция точек ByDay</summary>
        private SeriesCollection _ByDaySeries;

        /// <summary>Коллекция точек ByDay</summary>
        public SeriesCollection ByDaySeries
        {
            get => _ByDaySeries;
            set => Set(ref _ByDaySeries, value);
        }

        #endregion


        #region Period : PeriodUnits - Период на графике

        /// <summary>Период на графике</summary>
        private PeriodUnits _Period = PeriodUnits.Days;

        /// <summary>Период на графике</summary>
        public PeriodUnits Period
        {
            get => _Period;
            set => Set(ref _Period, value);
        }

        #endregion

        #region InitialDateTime : DateTime - Начальное время

        /// <summary>Начальное время</summary>
        private DateTime _InitialDateTime = DateTime.UtcNow.Date.AddDays(-7);

        /// <summary>Начальное время</summary>
        public DateTime InitialDateTime
        {
            get => _InitialDateTime;
            set => Set(ref _InitialDateTime, value);
        }

        #endregion

        #region GraphMaxValue : int - Максимальное значение даты на графике

        /// <summary>Максимальное значение даты на графике</summary>
        private int _GraphMaxValue = 8;

        /// <summary>Максимальное значение даты на графике</summary>
        public int GraphMaxValue
        {
            get => _GraphMaxValue;
            set => Set(ref _GraphMaxValue, value);
        }

        #endregion

        #region GraphMinValue : int - Минимальное значение даты на графике

        /// <summary>Минимальное значение даты на графике</summary>
        private int _GraphMinValue = 0;

        /// <summary>Минимальное значение даты на графике</summary>
        public int GraphMinValue
        {
            get => _GraphMinValue;
            set => Set(ref _GraphMinValue, value);
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

        #region NavigateToHomeViewCommand

        public ICommand NavigateToHomeViewCommand { get; }
        private bool CanNavigateToHomeViewCommandExecute(object p) => true;

        private void OnNavigateToHomeViewCommandExecuted(object p)
        {
            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<HomeView>());
        }

        #endregion

    }
}
