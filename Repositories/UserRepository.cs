using System;
using System.Collections.Generic;
using System.Linq;
using Market_App.Enums;
using Market_App.Extensions;
using Market_App.IRepository;
using Market_App.Service;
using Npgsql;

namespace Market_App.Models
{
    internal class UserRepository : IUserRepository
    {
        private string query;
        NpgsqlConnection connection = new NpgsqlConnection(Constants.CONNECTION_STRING);
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;
        public void Create(User user)
        {
            query = $"INSERT INTO Users (first_name, last_name, username, password, created_at, role) " +
            $"values ('{user.FirstName}', '{user.LastName}', '{user.Login}', '{user.Password}', Now(), '{user.Role}');";

            cmd = new NpgsqlCommand(query, connection);
            
            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public IList<User> GetAllUsers()
        {
            IList<User> users = new List<User>();

            query = "SELECT * FROM Users;";

            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader[1].ToString(),
                    LastName = reader[2].ToString(),
                    Login = reader[3].ToString(),
                    Password = reader[4].ToString(),
                    Created = reader.GetDateTime(5),
                    Role = (UserRole)Enum.Parse(typeof(UserRole), reader[6].ToString())
                });
            }

            connection.Close();

            return users;
        }

        public User GetUser(int id)
        {
            User user = new User();
            query = $"SELECT * FROM Users WHERE id = {id};";

            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetInt32(0);
                user.FirstName = reader[1].ToString();
                user.LastName = reader[2].ToString();
                user.Login = reader[3].ToString();
                user.Password = reader[4].ToString();
                user.Created = reader.GetDateTime(5);
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), reader[6].ToString());
            };

            connection.Close();

            return user;
        }

        public User Login(SignIn signIn)
        {
            User user = new User();

            query = $"SELECT * FROM Users WHERE username = '{signIn.Login}' and password = '{signIn.Password}';";

            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                user.Id = reader.GetInt32(0);
                    user.FirstName = reader[1].ToString();
                    user.LastName = reader[2].ToString();
                    user.Login = reader[3].ToString();
                    user.Password = reader[4].ToString();
                    user.Created = reader.GetDateTime(5);
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), reader[6].ToString());
            };

            connection.Close();

            return user;
        }

        public void EditUser(User user)
        {
            query = $"UPDATE Users SET first_name = '{user.FirstName}'," +
            $" last_name = '{user.LastName}', " +
            $" username = '{user.Login}', " +
            $" password = '{user.Password}' WHERE id = {user.Id}";

            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void RemoveUser(User user)
        {
            query = $"DELETE FROM Users WHERE id = {user.Id}";

            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
