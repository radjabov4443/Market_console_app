using Market_App.Enums;
using Market_App.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_App.Service
{
    sealed class DbContext
    {
        public NpgsqlConnection connection = new NpgsqlConnection(Constants.CONNECTION_STRING);
        
        NpgsqlCommand cmd;
        
        public void Connect(string query)
        {
            cmd = new NpgsqlCommand(query, connection);

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
