using Market_App.Enums;
using Market_App.IRepository;
using Market_App.Models;
using Market_App.Service;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Market_App.Repositories
{
    internal class HistoryRepository : IHistoryRepository
    {

        private string query;

        public NpgsqlCommand cmd;
        
        public NpgsqlDataReader reader;

        DbContext _Db = new DbContext();

        public void Create(History history)
        {
            query = $"INSERT INTO products_sold (product_id, user_id, quantity)" +
                $"VALUES ({history.ProductId}, {history.CustomerId}, {history.Quantity})";

            _Db.Connect(query);
        }

        public List<History> GetHistories()
        {
            IList<History> histories = new List<History>();
            query = "SELECT * FROM History";

            cmd = new NpgsqlCommand(query, _Db.connection);

            _Db.connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                histories.Add(new History
                {
                    CustomerId = (int)reader[0],
                    CustomerFullName = reader[1].ToString(),
                    ProductName = reader[2].ToString(),
                    ProductPrice = (decimal)reader[3],
                    Quantity = reader.GetFloat(4),
                    Unit = (Unit)Enum.Parse(typeof(Unit), reader[5].ToString()),
                    Type = (ProductsType)Enum.Parse(typeof(ProductsType), reader[6].ToString()),
                    Date = (DateTime)reader.GetDate(7) + reader.GetTimeSpan(8),
                });
            }

            _Db.connection.Close();

            return (List<History>)histories;
        }
    }
}
