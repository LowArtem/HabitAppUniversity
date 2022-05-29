using HabitApp.Data;
using HabitApp.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabitApp.Implementation
{
    public class DailyHabitRepository : IRepository<DailyHabit>
    {
        private const string connectionString = "User Id=postgres;Password=root;Host=localhost;Port=5432;Database=habit_db;";

        public DailyHabit Add(DailyHabit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"insert into daily_habits (name, description, status, difficulty, category, deadline, userid) values ('{entity.Name}', '{entity.Description}', {entity.Status}, {entity.Difficulty}, '{entity.Category}', {entity.Deadline}, {entity.UserId}) returning id";
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                entity.Id = reader.GetInt32(0);
                reader.Close();
            }

            con.Close();
            return entity;
        }

        public void Delete(DailyHabit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"delete from daily_habits where id = {entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
        }

        public List<DailyHabit> GetAll()
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from daily_habits";
            var reader = command.ExecuteReader();

            List<DailyHabit> daily_habits = new List<DailyHabit>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DailyHabit daily_habit = new DailyHabit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));

                    daily_habits.Add(daily_habit);
                }
                reader.Close();
            }

            con.Close();
            return daily_habits;
        }

        public DailyHabit GetById(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from daily_habits where id = {id}";
            var reader = command.ExecuteReader();

            DailyHabit daily_habit = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    daily_habit = new DailyHabit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));
                }
                reader.Close();
            }

            con.Close();
            return daily_habit;
        }

        public DailyHabit Update(DailyHabit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"update daily_habits set name='{entity.Name}', description='{entity.Description}', category='{entity.Category}', status={entity.Status}, difficulty={entity.Difficulty}, userid={entity.UserId}, deadline={entity.Deadline} where id={entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
            return entity;
        }

        public List<DailyHabit> GetAllByUserId(int userid)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from daily_habits where userid={userid}";
            var reader = command.ExecuteReader();

            List<DailyHabit> daily_habits = new List<DailyHabit>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DailyHabit daily_habit = new DailyHabit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));

                    daily_habits.Add(daily_habit);
                }
                reader.Close();
            }

            con.Close();
            return daily_habits;
        }

        public List<HabitCompletion> GetAllCompletionsById(int habitId)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select * from daily_completions
                                     where dailyid={habitId}";

            var reader = command.ExecuteReader();

            List<HabitCompletion> completions = new List<HabitCompletion>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    HabitCompletion habit = new HabitCompletion
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         date: Convert.ToDateTime(reader["date"].ToString()),
                         rating: Convert.ToInt32(reader["rating"].ToString()),
                         habitId: Convert.ToInt32(reader["dailyid"].ToString()));

                    completions.Add(habit);
                }
                reader.Close();
            }

            con.Close();
            return completions;
        }

        public void AddHabitCompletion(int habitId, DateTime date, int rating)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"insert into daily_completions 
                                    (date, rating, dailyid) values
                                    ('{date}', {rating}, {habitId});";
            command.ExecuteNonQuery();

            con.Close();
        }
    }
}
