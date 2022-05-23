using HabitApp.Repositories;
using System;
using System.Collections.Generic;
using HabitApp.Data;
using Npgsql;
using System.Data;

namespace HabitApp.Implementation
{
    public class UserRepository : IRepository<User>
    {
        private const string connectionString = "User=root;Password=root;Host=localhost;Port=5432;Database=habit_db;";

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
            command.CommandText = $"insert into users (username, experience, password, money, groupid) values ({entity.Username}, {entity.Experience}, {entity.Password}, {entity.Money}, {entity.GroupId}) returning id";
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                entity.Id = reader.GetInt32(0);
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
                while(reader.Read())
                {
                    int? groupid = null;
                    if (reader["groupid"] != null)
                        groupid = Convert.ToInt32(reader["groupid"].ToString());

                    User user = new User
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         username: reader["username"].ToString(),
                         password: reader["password"].ToString(),
                         experience: Convert.ToInt64(reader["experience"].ToString()),
                         money: Convert.ToInt64(reader["money"].ToString()),
                         groupId: groupid);

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
                    int? groupid = null;
                    if (reader["groupid"] != null)
                        groupid = Convert.ToInt32(reader["groupid"].ToString());

                    user = new User
                        (id: Convert.ToInt32(reader["id"].ToString()),
                         username: reader["username"].ToString(),
                         password: reader["password"].ToString(),
                         experience: Convert.ToInt64(reader["experience"].ToString()),
                         money: Convert.ToInt64(reader["money"].ToString()),
                         groupId: groupid);
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
            command.CommandText = $"update users set username={entity.Username} set experience={entity.Experience} set password={entity.Password} set money={entity.Money} set groupid={entity.GroupId} where id={entity.Id}";
            command.ExecuteNonQuery();

            con.Close();
            return entity;
        }
    }
}
