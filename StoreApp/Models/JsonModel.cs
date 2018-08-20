using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class JsonModel
    {
        public int orderId;
        public int userId;
        public string username;
        public double sum;
        public List<UserProduct> products = new List<UserProduct>();

    }
}
