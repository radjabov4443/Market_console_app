using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Market_App.Service;
using Market_App.IRepository;
using Npgsql;
using Market_App.Enums;
using Market_App.Registration;

namespace Market_App.Models
{
    internal class ProductRepository : IProductRepository
    {
        string query;

        DbContext _Db = new DbContext();

        public NpgsqlCommand cmd;

        public NpgsqlDataReader reader;

        IList<Product> Products = new List<Product>();

        public IList<Product> Get()
        {
            query = "SELECT * FROM Products ORDER BY id asc;";

            cmd = new NpgsqlCommand(query, _Db.connection);

            _Db.connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader[1].ToString(),
                    Price = reader.GetDecimal(2),
                    Unit = (Unit)Enum.Parse(typeof(Unit), reader[3].ToString()),
                    Residue = reader.GetFloat(4),
                    Type = (ProductsType)Enum.Parse(typeof(ProductsType), reader[5].ToString()),
                    Status = (ProductStatus)Enum.Parse(typeof(ProductStatus), reader[6].ToString())
                });
            }

            _Db.connection.Close();

            return Products;
        }

        public Product Get(string name)
        {
            query = $"SELECT * FROM Products WHERE name = '{name}';";

            cmd = new NpgsqlCommand(query, _Db.connection);

            _Db.connection.Open();

            reader = cmd.ExecuteReader();

            Product product = new Product();

            while (reader.Read())
            {
                product.Id = reader.GetInt32(0);
                product.Name = reader.GetString(1);
                product.Price = reader.GetInt32(2);
                product.Unit = (Unit)Enum.Parse(typeof(Unit), reader[3].ToString());
                product.Residue = reader.GetFloat(4);
                product.Type = (ProductsType)Enum.Parse(typeof(ProductsType), reader[5].ToString());
            };

            _Db.connection.Close();

            return product.Name == null ? null : product;
        }

        public Product Get(int id)
        {
            query = $"SELECT * FROM Products WHERE id = {id});";

            cmd = new NpgsqlCommand(query, _Db.connection);

            _Db.connection.Open();

            reader = cmd.ExecuteReader();

            Product product = new Product();

            while (reader.Read())
            {
                product.Id = reader.GetInt32(0);
                product.Name = reader.GetString(1);
                product.Price = reader.GetInt32(2);
                product.Unit = (Unit)reader[3];
                product.Residue = reader.GetInt32(4);
                product.Type = (ProductsType)reader[5];
                product.Status = (ProductStatus)reader[6];
            };

            _Db.connection.Close();

            return product;
        }

        public IList<Product> Search(string name)
        {
            query = $"SELECT * FROM Products WHERE name LIKE '%{name}' OR name LIKE '{name}%' OR name LIKE '%{name}%'";

            cmd = new NpgsqlCommand(query, _Db.connection);

            _Db.connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader[1].ToString(),
                    Price = reader.GetDecimal(2),
                    Unit = (Unit)Enum.Parse(typeof(Unit), reader[3].ToString()),
                    Residue = reader.GetFloat(4),
                    Type = (ProductsType)Enum.Parse(typeof(ProductsType), reader[5].ToString()),
                    Status = (ProductStatus)Enum.Parse(typeof(ProductStatus), reader[6].ToString())
                });
            }

            _Db.connection.Close();

            return Products;
        }

        public void Remove(int id)
        {
            query = $"DELETE FROM Products WHERE id = {id};";

            _Db.Connect(query);
        }

        public void Update(Product product)
        {
            query = $"UPDATE Products SET name = '{product.Name}'," +
                $" price = '{product.Price}'," +
                $" unit = '{product.Unit}'," +
                $" residue = '{product.Residue}'," +
                $" type = '{product.Type}'," +
                $" status = '{product.Status}' WHERE id = {product.Id}";

            _Db.Connect(query);
        }

        public void Calculation(Product product)
        {
            query = $"UPDATE Products" +
                    $" SET residue = ((SELECT residue FROM Products WHERE id = {product.Id}) - {product.Residue}) WHERE id = {product.Id};";

            _Db.Connect(query);
        }

        public void Create(Product product)
        {
            query = $"INSERT INTO Products (name, price, unit, residue, type, status, user_id)" +
                $"VALUES('{product.Name}', '{product.Price}', '{product.Unit}', '{product.Residue}', '{product.Type}', '{product.Status}', {MainMenu.us.Id});";

            _Db.Connect(query);
        }

    }
}