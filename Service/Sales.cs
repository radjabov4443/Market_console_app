using Market_App.IRepository;
using Market_App.Service;
using System.Collections.Generic;
using System.Linq;

namespace Market_App.Models
{
    internal class Sales
    {
        private IList<Product> SellingProducts()
        {
            IProductRepository prodRepo = new ProductRepository();
            var _products = prodRepo.Get();

            foreach (var res in _products)
            {
                if (res.Price < 8000)
                    res.Price += 500;
                else if (res.Price < 20000)
                    res.Price += 2000;
                else if (res.Price < 50000)
                    res.Price += 5000;
                else if (res.Price < 80000)
                    res.Price += 10000;
            }
            return _products;
        }

        public IList<Product> GetProductsForSelling()
        {
            return SellingProducts();
        }
    }
}
