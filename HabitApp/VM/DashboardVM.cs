using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
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

            ByDaySeries = new ISeries[]
            {
                new LineSeries<DateTimePoint>
                {
                    Name = "Habits",
                    TooltipLabelFormatter = (chartPoint) => $"Habits at {new DateTime((long) chartPoint.SecondaryValue):dd.MM}: {chartPoint.PrimaryValue}",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                    GeometrySize = 12,
                    Values = HabitsCountByDay
                },
                new LineSeries<DateTimePoint>
                {
                    Name = "Daily Habits",
                    TooltipLabelFormatter = (chartPoint) => $"Daily habits at {new DateTime((long) chartPoint.SecondaryValue):dd.MM}: {chartPoint.PrimaryValue}",
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 3 },
                    GeometrySize = 12,
                    Values = DailyHabitsCountByDay
                },
                new LineSeries<DateTimePoint>
                {
                    Name = "Tasks",
                    TooltipLabelFormatter = (chartPoint) => $"Tasks at {new DateTime((long) chartPoint.SecondaryValue):dd.MM}: {chartPoint.PrimaryValue}",
                    Stroke = new SolidColorPaint(SKColors.GreenYellow) { StrokeThickness = 3 },
                    GeometrySize = 12,
                    Values = TasksCountByDay
                }
            };
        }

        public Axis[] DayXAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("dd.MM"),
                LabelsRotation = 90,

                // when using a date time type, let the library know your unit 
                UnitWidth = TimeSpan.FromDays(1).Ticks, 

                // if the difference between our points is in hours then we would:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                // Months: TimeSpan.FromDays(30.4375).Ticks
                // Years: TimeSpan.FromDays(365.25).Ticks

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

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
            List<DateTimePoint> habitsCollection,
            List<DateTimePoint> dailyHabitsCollection,
            List<DateTimePoint> tasksCollection)
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
                habitsCollection.Add(new DateTimePoint(d, _statisticsService.GetHabitsCompletionsCountForPeriod(userId, period, d)));
                dailyHabitsCollection.Add(new DateTimePoint(d, _statisticsService.GetDailyHabitsCompletionsCountForPeriod(userId, period, d)));
                tasksCollection.Add(new DateTimePoint(d, _statisticsService.GetTasksCompletionsCountForPeriod(userId, period, d)));
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

        #region HabitsCountByDay : List<DateTimePoint> - Список количества завершённых привычек за день

        /// <summary>Список количества завершённых привычек за день</summary>
        private List<DateTimePoint> _HabitsCountByDay = new List<DateTimePoint>();

        /// <summary>Список количества завершённых привычек за день</summary>
        public List<DateTimePoint> HabitsCountByDay
        {
            get => _HabitsCountByDay;
            set => Set(ref _HabitsCountByDay, value);
        }

        #endregion

        #region DailyHabitsCountByDay : List<DateTimePoint> - Список количества завершённых ежедневных привычек за день

        /// <summary>Список количества завершённых ежедневных привычек за день</summary>
        private List<DateTimePoint> _DailyHabitsCountByDay = new List<DateTimePoint>();

        /// <summary>Список количества завершённых ежедневных привычек за день</summary>
        public List<DateTimePoint> DailyHabitsCountByDay
        {
            get => _DailyHabitsCountByDay;
            set => Set(ref _DailyHabitsCountByDay, value);
        }

        #endregion

        #region TasksCountByDay : List<DateTimePoint> - Список количества завершённых задач за день

        /// <summary>Список количества завершённых задач за день</summary>
        private List<DateTimePoint> _TasksCountByDay = new List<DateTimePoint>();

        /// <summary>Список количества завершённых задач за день</summary>
        public List<DateTimePoint> TasksCountByDay
        {
            get => _TasksCountByDay;
            set => Set(ref _TasksCountByDay, value);
        }

        #endregion

        #region HabitsCountByWeek : List<DateTimePoint> - Список количества завершённых привычек за неделю

        /// <summary>Список количества завершённых привычек за неделю</summary>
        private List<DateTimePoint> _HabitsCountByWeek = new List<DateTimePoint>();

        /// <summary>Список количества завершённых привычек за неделю</summary>
        public List<DateTimePoint> HabitsCountByWeek
        {
            get => _HabitsCountByWeek;
            set => Set(ref _HabitsCountByWeek, value);
        }

        #endregion

        #region DailyHabitsCountByWeek : List<DateTimePoint> - Список количества завершённых ежедневных привычек за неделю

        /// <summary>Список количества завершённых ежедневных привычек за неделю</summary>
        private List<DateTimePoint> _DailyHabitsCountByWeek = new List<DateTimePoint>();

        /// <summary>Список количества завершённых ежедневных привычек за неделю</summary>
        public List<DateTimePoint> DailyHabitsCountByWeek
        {
            get => _DailyHabitsCountByWeek;
            set => Set(ref _DailyHabitsCountByWeek, value);
        }

        #endregion

        #region TasksCountByWeek : List<DateTimePoint> - Список количества завершённых задач за неделю

        /// <summary>Список количества завершённых задач за неделю</summary>
        private List<DateTimePoint> _TasksCountByWeek = new List<DateTimePoint>();

        /// <summary>Список количества завершённых задач за неделю</summary>
        public List<DateTimePoint> TasksCountByWeek
        {
            get => _TasksCountByWeek;
            set => Set(ref _TasksCountByWeek, value);
        }

        #endregion

        #region HabitsCountByMonth : List<DateTimePoint> - Список количества завершённых привычек за месяц

        /// <summary>Список количества завершённых привычек за месяц</summary>
        private List<DateTimePoint> _HabitsCountByMonth = new List<DateTimePoint>();

        /// <summary>Список количества завершённых привычек за месяц</summary>
        public List<DateTimePoint> HabitsCountByMonth
        {
            get => _HabitsCountByMonth;
            set => Set(ref _HabitsCountByMonth, value);
        }

        #endregion

        #region DailyHabitsCountByMonth : List<DateTimePoint> - Список количества завершённых ежедневных привычек за месяц

        /// <summary>Список количества завершённых ежедневных привычек за месяц</summary>
        private List<DateTimePoint> _DailyHabitsCountByMonth = new List<DateTimePoint>();

        /// <summary>Список количества завершённых ежедневных привычек за месяц</summary>
        public List<DateTimePoint> DailyHabitsCountByMonth
        {
            get => _DailyHabitsCountByMonth;
            set => Set(ref _DailyHabitsCountByMonth, value);
        }

        #endregion

        #region TasksCountByMonth : List<DateTimePoint> - Список количества завершённых задач за месяц

        /// <summary>Список количества завершённых задач за месяц</summary>
        private List<DateTimePoint> _TasksCountByMonth = new List<DateTimePoint>();

        /// <summary>Список количества завершённых задач за месяц</summary>
        public List<DateTimePoint> TasksCountByMonth
        {
            get => _TasksCountByMonth;
            set => Set(ref _TasksCountByMonth, value);
        }

        #endregion

        #region HabitsCountByYear : List<DateTimePoint> - Список количества завершённых привычек за год

        /// <summary>Список количества завершённых привычек за год</summary>
        private List<DateTimePoint> _HabitsCountByYear = new List<DateTimePoint>();

        /// <summary>Список количества завершённых привычек за год</summary>
        public List<DateTimePoint> HabitsCountByYear
        {
            get => _HabitsCountByYear;
            set => Set(ref _HabitsCountByYear, value);
        }

        #endregion

        #region DailyHabitsCountByYear : List<DateTimePoint> - Список количества завершённых ежедневных задач за год

        /// <summary>Список количества завершённых ежедневных задач за год</summary>
        private List<DateTimePoint> _DailyHabitsCountByYear = new List<DateTimePoint>();

        /// <summary>Список количества завершённых ежедневных задач за год</summary>
        public List<DateTimePoint> DailyHabitsCountByYear
        {
            get => _DailyHabitsCountByYear;
            set => Set(ref _DailyHabitsCountByYear, value);
        }

        #endregion

        #region TasksCountByYear : List<DateTimePoint> - Список количества завершённых задач за год

        /// <summary>Список количества завершённых задач за год</summary>
        private List<DateTimePoint> _TasksCountByYear = new List<DateTimePoint>();

        /// <summary>Список количества завершённых задач за год</summary>
        public List<DateTimePoint> TasksCountByYear
        {
            get => _TasksCountByYear;
            set => Set(ref _TasksCountByYear, value);
        }

        #endregion

        #endregion


        #region ByDaySeries : ISeries[] - Коллекция точек ByDay

        /// <summary>Коллекция точек ByDay</summary>
        private ISeries[] _ByDaySeries;

        /// <summary>Коллекция точек ByDay</summary>
        public ISeries[] ByDaySeries
        {
            get => _ByDaySeries;
            set => Set(ref _ByDaySeries, value);
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
