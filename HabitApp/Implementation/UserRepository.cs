using HabitApp.Data;
using HabitApp.Model;
using HabitApp.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HabitApp.Implementation
{
    public class UserRepository : IRepository<User>
    {
        private readonly string connectionString = Properties.Settings.Default.connectionString;

        public User Add(User entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;

            if (entity.GroupId != null)
                command.CommandText = $"insert into users (username, experience, password, money, groupid) values ('{entity.Username}', {entity.Experience}, '{entity.Password}', {entity.Money}, {entity.GroupId}) returning id";
            else
                command.CommandText = $"insert into users (username, experience, password, money, groupid) values ('{entity.Username}', {entity.Experience}, '{entity.Password}', {entity.Money}, null) returning id";

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entity.Id = Convert.ToInt32(reader["id"].ToString());
                }
                reader.Close();
            }

            con.Close();
            return entity;
        }

        public void Delete(User entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"delete from users where id = {entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
        }

        public List<User> GetAll()
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from users";
            var reader = command.ExecuteReader();

            List<User> users = new List<User>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User user = new User
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         username: reader["username"].ToString(),
                         password: reader["password"].ToString(),
                         experience: Convert.ToInt64(reader["experience"].ToString()),
                         money: Convert.ToInt64(reader["money"].ToString()),
                         groupId: Extensions.ToNullableInt(reader["groupid"].ToString()));

                    users.Add(user);
                }
                reader.Close();
            }

            con.Close();
            return users;
        }

        public User GetById(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from users where id = {id}";
            var reader = command.ExecuteReader();

            User user = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user = new User
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         username: reader["username"].ToString(),
                         password: reader["password"].ToString(),
                         experience: Convert.ToInt64(reader["experience"].ToString()),
                         money: Convert.ToInt64(reader["money"].ToString()),
                         groupId: Extensions.ToNullableInt(reader["groupid"].ToString()));
                }
                reader.Close();
            }

            con.Close();
            return user;
        }

        public User Update(User entity)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;

            string groupid = entity.GroupId == null ? "null" : entity.GroupId.ToString();

            command.CommandText = $"update users set username='{entity.Username}', experience={entity.Experience}, password='{entity.Password}', money={entity.Money}, groupid={groupid} where id={entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
            return entity;
        }

        public User GetUserByCredentials(string username, string password)
        {
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            if (con.FullState == ConnectionState.Broken || con.FullState == ConnectionState.Closed)
            {
                throw new Exception("Не работает соединение с бд");
            }

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = con;
            command.CommandText = $"select * from users where username='{username}' and password='{password}'";
            var reader = command.ExecuteReader();

            User user = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user = new User
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         username: reader["username"].ToString(),
                         password: reader["password"].ToString(),
                         experience: Convert.ToInt64(reader["experience"].ToString()),
                         money: Convert.ToInt64(reader["money"].ToString()),
                         groupId: Extensions.ToNullableInt(reader["groupid"].ToString()));
                }
                reader.Close();
            }

            con.Close();
            return user;
        }
    }
}
