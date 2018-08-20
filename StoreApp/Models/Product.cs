using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class Product
    { 
        public int ID { get; set; }

        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string ProductType{ get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public int SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
