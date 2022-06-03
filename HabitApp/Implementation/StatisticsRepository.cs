using LiveChartsCore.Defaults;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabitApp.Implementation
{
    public class StatisticsRepository
    {
        private readonly string connectionString = Properties.Settings.Default.connectionString;


        #region HabitCompletions
        public int GetHabitsCompletionsCountForDay(int userId, DateTime day)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from habits 
                                    join habit_completions on habits.id=habit_completions.habitid and habit_completions.date between Date('{day}') and Date('{day.AddDays(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetHabitsCompletionsCountForWeek(int userId, DateTime firstDayOfWeek)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from habits 
                                    join habit_completions on habits.id=habit_completions.habitid and habit_completions.date between Date('{firstDayOfWeek}') and Date('{firstDayOfWeek.AddDays(7)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetHabitsCompletionsCountForMonth(int userId, DateTime firstDayOfMonth)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from habits 
                                    join habit_completions on habits.id=habit_completions.habitid and habit_completions.date between Date('{firstDayOfMonth}') and Date('{firstDayOfMonth.AddMonths(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetHabitsCompletionsCountForYear(int userId, DateTime firstDayOfYear)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from habits 
                                    join habit_completions on habits.id=habit_completions.habitid and habit_completions.date between Date('{firstDayOfYear}') and Date('{firstDayOfYear.AddYears(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }
        #endregion

        #region DailyHabitCompletions
        public int GetDailyHabitsCompletionsCountForDay(int userId, DateTime day)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from daily_habits 
                                    join daily_completions on daily_habits.id=daily_completions.dailyid and daily_completions.date between Date('{day}') and Date('{day.AddDays(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetDailyHabitsCompletionsCountForWeek(int userId, DateTime firstDayOfWeek)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from daily_habits 
                                    join daily_completions on daily_habits.id=daily_completions.dailyid and daily_completions.date between Date('{firstDayOfWeek}') and Date('{firstDayOfWeek.AddDays(7)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetDailyHabitsCompletionsCountForMonth(int userId, DateTime firstDayOfMonth)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from daily_habits 
                                    join daily_completions on daily_habits.id=daily_completions.dailyid and daily_completions.date between Date('{firstDayOfMonth}') and Date('{firstDayOfMonth.AddMonths(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetDailyHabitsCompletionsCountForYear(int userId, DateTime firstDayOfYear)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from daily_habits 
                                    join daily_completions on daily_habits.id=daily_completions.dailyid and daily_completions.date between Date('{firstDayOfYear}') and Date('{firstDayOfYear.AddYears(1)}')
                                    where userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        #endregion

        #region TasksCompletions
        public int GetTasksCompletionsCountForDay(int userId, DateTime day)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from tasks 
                                    join task_to_user on tasks.id=task_to_user.taskid and task_to_user.date between Date('{day}') and Date('{day.AddDays(1)}')
                                    where tasks.status=true and task_to_user.userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetTasksCompletionsCountForWeek(int userId, DateTime firstDayOfWeek)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from tasks 
                                    join task_to_user on tasks.id=task_to_user.taskid and task_to_user.date between Date('{firstDayOfWeek}') and Date('{firstDayOfWeek.AddDays(7)}')
                                    where tasks.status=true and task_to_user.userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetTasksCompletionsCountForMonth(int userId, DateTime firstDayOfMonth)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from tasks 
                                    join task_to_user on tasks.id=task_to_user.taskid and task_to_user.date between Date('{firstDayOfMonth}') and Date('{firstDayOfMonth.AddMonths(1)}')
                                    where tasks.status=true and task_to_user.userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetTasksCompletionsCountForYear(int userId, DateTime firstDayOfYear)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) as cnt from tasks 
                                    join task_to_user on tasks.id=task_to_user.taskid and task_to_user.date between Date('{firstDayOfYear}') and Date('{firstDayOfYear.AddYears(1)}')
                                    where tasks.status=true and task_to_user.userid={userId}";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["cnt"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }
        #endregion


        #region RatingsReview

        public List<DateTimePoint> GetAllRatingsWithDates(int userId)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"with all_ratings as
                                    (
	                                    select rating, date from habit_completions
	                                    join habits on habitid=habits.id and userid={userId}
	                                    where Date(date) <= Date(current_date)
	                                    union all
	                                    select rating, date from daily_completions
	                                    join daily_habits on dailyid=daily_habits.id and userid={userId}
	                                    where Date(date) <= Date(current_date)
                                    )
                                    select * from all_ratings
                                    order by date asc;";

            var reader = command.ExecuteReader();

            List<DateTimePoint> ratings = new List<DateTimePoint>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DateTimePoint rating = new DateTimePoint
                        (dateTime: Convert.ToDateTime(reader["date"].ToString()),
                         value: Convert.ToInt32(reader["rating"].ToString()));

                    ratings.Add(rating);
                }
                reader.Close();
            }

            con.Close();
            return ratings;
        }

        #endregion

        #region MiniStats

        public int GetTotalCompletionsCount(int userId)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"with completed as 
                                    ((select habits.id as cnt from habits 
                                      join habit_completions on habits.id=habit_completions.habitid
                                      where userid={userId} and Date(habit_completions.date) <= Date(current_date)) 
                                    union all
                                    (select daily_habits.id as cnt from daily_habits 
                                     join daily_completions on daily_habits.id=daily_completions.dailyid
                                     where userid={userId} and Date(daily_completions.date) <= Date(current_date)) 
                                    union all
                                    (select tasks.id as cnt from tasks 
                                     join task_to_user on tasks.id=task_to_user.taskid
                                     where tasks.status=true and task_to_user.userid={userId} and Date(task_to_user.date) <= Date(current_date)))
 
                                    select count(*) from completed;";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["count"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetTodayCompletedHabitsCount(int userId)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) from habits
                                    join habit_completions on habitid = habits.id
                                    where userid={userId} and Date(date) = Date(current_date);";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["count"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        public int GetCurrentCompletedTasksCount(int userId)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select count(*) from tasks
                                    join task_to_user on taskid=tasks.id
                                    where userid={userId} and status=true";

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["count"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return result;
        }

        #endregion
    }
}
