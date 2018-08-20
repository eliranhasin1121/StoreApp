using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class Supplier
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        virtual public ICollection<Product> Products { get; set; }
    }
}
