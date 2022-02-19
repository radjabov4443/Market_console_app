using Market_App.Enums;
using System;

namespace Market_App.Models
{
    internal class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public Unit Unit { get; set; }
        
        public float Residue { get; set; }
        
        public ProductsType Type { get; set; }

        public ProductStatus Status { get; set; }
        public DateTime Date { get; internal set; }
    }
}
