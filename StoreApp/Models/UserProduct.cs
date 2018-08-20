using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class UserProduct
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string ProductType { get; set; }
        public double Price { get; set; }
    }
}
