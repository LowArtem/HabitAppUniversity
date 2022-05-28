using HabitApp.Data;
using HabitApp.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabitApp.Implementation
{
    public class HabitRepository : IRepository<Habit>
    {
        private const string connectionString = "User Id=postgres;Password=root;Host=localhost;Port=5432;Database=habit_db;";

        public Habit Add(Habit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"insert into habits (name, description, category, type, difficulty, userid) values ('{entity.Name}', '{entity.Description}', '{entity.Category}', {entity.Type}, {entity.Difficulty}, {entity.UserId}) returning id";
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                entity.Id = reader.GetInt32(0);
                reader.Close();
            }

            con.Close();
            return entity;
        }

        public void Delete(Habit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"delete from habits where id = {entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
        }

        public List<Habit> GetAll()
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from habits";
            var reader = command.ExecuteReader();

            List<Habit> habits = new List<Habit>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Habit habit = new Habit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         type: Convert.ToInt32(reader["type"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));

                    habits.Add(habit);
                }
                reader.Close();
            }

            con.Close();
            return habits;
        }

        public Habit GetById(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from habits where id = {id}";
            var reader = command.ExecuteReader();

            Habit habit = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {                    
                    habit = new Habit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         type: Convert.ToInt32(reader["type"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));
                }
                reader.Close();
            }

            con.Close();
            return habit;
        }

        public Habit Update(Habit entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"update habits set name='{entity.Name}' set description='{entity.Description}' set category='{entity.Category}' set type={entity.Type} set difficulty={entity.Difficulty} set userid={entity.UserId} where id={entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
            return entity;
        }

        public List<Habit> GetAllByUserId(int userid)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from habits where userid={userid}";
            var reader = command.ExecuteReader();

            List<Habit> habits = new List<Habit>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Habit habit = new Habit
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         category: reader["category"].ToString(),
                         type: Convert.ToInt32(reader["type"].ToString()),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         userId: Convert.ToInt32(reader["userid"].ToString()));

                    habits.Add(habit);
                }
                reader.Close();
            }

            con.Close();
            return habits;
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
            command.CommandText = $@"select * from habit_completions
                                     where habitid={habitId}";

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
                         habitId: Convert.ToInt32(reader["habitid"].ToString()));

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
            command.CommandText = $@"insert into habit_completions 
                                    (date, rating, habitid) values
                                    ('{date}', {rating}, {habitId});";
            command.ExecuteNonQuery();

            con.Close();
        }
    }
}
