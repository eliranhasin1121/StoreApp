using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class StorageProducts
    {
        public StorageProducts() {  }
        public StorageProducts(int prodId, string productName, Supplier supplier)
        {
            this.ProductID = prodId;
            this.ProductName = productName;
            this.Supplier = supplier;
            this.Amount = 100;
            this.LastOrder = DateTime.Now;
        }
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime LastOrder { get; set; }
        public virtual Supplier Supplier { get; set; }
        public int Amount { get; set; }
    }
}
