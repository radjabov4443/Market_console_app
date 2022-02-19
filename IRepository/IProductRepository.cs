using Market_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market_App.IRepository
{
    interface IProductRepository
    {
        IList<Product> Get();

        Product Get(int id);

        Product Get(string name);
        
        IList<Product> Search(string name);

        void Create(Product product);
        
        void Update(Product product);
        void Remove(int id);

        void Calculation(Product product);

    }
}
