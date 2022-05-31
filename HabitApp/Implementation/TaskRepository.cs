using HabitApp.Data;
using HabitApp.Model;
using HabitApp.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabitApp.Implementation
{
    public class TaskRepository : IRepository<Task>
    {
        private const string connectionString = "User Id=postgres;Password=root;Host=localhost;Port=5432;Database=habit_db;";

        public Task Add(Task entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;

            string userEventId;
            string groupdEventId;

            if (entity.UserEventId == null)
                userEventId = "NULL";
            else
                userEventId = entity.UserEventId.ToString();

            if (entity.GroupEventId == null)
                groupdEventId = "NULL";
            else
                groupdEventId = entity.GroupEventId.ToString();

            command.CommandText = $"insert into tasks (name, description, priority, status, difficulty, groupeventid, usereventid, deadline) values ('{entity.Name}', '{entity.Description}', {entity.Priority}, {entity.Status}, {entity.Difficulty}, {groupdEventId}, {userEventId}, '{entity.Deadline}') returning id";
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                entity.Id = reader.GetInt32(0);
                reader.Close();
            }

            con.Close();
            return entity;
        }

        public void Delete(Task entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"delete from tasks where id = {entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
        }

        public List<Task> GetAll()
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from tasks";
            var reader = command.ExecuteReader();

            List<Task> tasks = new List<Task>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Task task = new Task
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         priority: Convert.ToInt32(reader["priority"].ToString()),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         groupEventId: Extensions.ToNullableInt(reader["groupeventid"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         userEventId: Extensions.ToNullableInt(reader["usereventid"].ToString()));

                    tasks.Add(task);
                }
                reader.Close();
            }

            con.Close();
            return tasks;
        }

        public Task GetById(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from tasks where id = {id}";
            var reader = command.ExecuteReader();

            Task task = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    task = new Task
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         priority: Convert.ToInt32(reader["priority"].ToString()),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         groupEventId: Extensions.ToNullableInt(reader["groupeventid"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         userEventId: Extensions.ToNullableInt(reader["usereventid"].ToString()));
                }
                reader.Close();
            }

            con.Close();
            return task;
        }

        public Task Update(Task entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;

            string userEventId;
            string groupdEventId;

            if (entity.UserEventId == null)
                userEventId = "NULL";
            else
                userEventId = entity.UserEventId.ToString();

            if (entity.GroupEventId == null)
                groupdEventId = "NULL";
            else
                groupdEventId = entity.GroupEventId.ToString();

            command.CommandText = $"update tasks set name='{entity.Name}', description='{entity.Description}', priority={entity.Priority}, status={entity.Status}, difficulty={entity.Difficulty}, groupeventid={groupdEventId}, usereventid={userEventId}, deadline='{entity.Deadline}' where id={entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
            return entity;
        }

        public List<Task> GetAllTasksByUser(int userid)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $@"select * from tasks where tasks.id in
                                    (select taskid from task_to_user
                                    where userid = {userid})";
            var reader = command.ExecuteReader();

            List<Task> tasks = new List<Task>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Task task = new Task
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         name: reader["name"].ToString(),
                         description: reader["description"].ToString(),
                         difficulty: Convert.ToInt32(reader["difficulty"].ToString()),
                         priority: Convert.ToInt32(reader["priority"].ToString()),
                         status: Convert.ToBoolean(reader["status"].ToString()),
                         groupEventId: Extensions.ToNullableInt(reader["groupeventid"].ToString()),
                         deadline: Convert.ToDateTime(reader["deadline"].ToString()),
                         userEventId: Extensions.ToNullableInt(reader["usereventid"].ToString()));

                    tasks.Add(task);
                }
                reader.Close();
            }

            con.Close();
            return tasks;
        }
    }
}
