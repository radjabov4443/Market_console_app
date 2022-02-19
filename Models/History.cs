using Market_App.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Market_App.Models
{
    internal class History
    {
        public int CustomerId { get; set; }

        public string CustomerFullName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public Unit Unit { get; set; }

        public ProductsType Type { get; set; }

        public DateTime Date { get; set; }
        
        public float Quantity { get; set; }

    }
}
