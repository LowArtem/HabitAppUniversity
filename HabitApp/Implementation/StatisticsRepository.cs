using HabitApp.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
