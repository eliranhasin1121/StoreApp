using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class ProductsController : Controller
    {
        static int orderId = 1;
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View();
        }
        [Route("products/productPage/{productName}")]
        public ActionResult productPage(String productName)
        {
            var meat = (from m in _context.Products
                        where m.ProductName == productName
                        select m);
            return View(meat.ToList());
        }
        [Route("products/searchBySupplier")]
        [HttpPost]
        public List<JsonResult> searchBySupplier(string supplierName, string prodtype, string partName)
        {

            int id = (from s in _context.Suppliers
                      where s.CompanyName == supplierName
                      select s.ID).SingleOrDefault<int>();

            List<JsonResult> result = new List<JsonResult>();
            var query = from sup in _context.Suppliers
                        join prod in _context.Products on sup.ID equals prod.SupplierID
                        where prod.SupplierID == sup.ID
                        select new { Supplier = sup, Product = prod };


            foreach (var item in query)
            {

                if (item.Supplier.CompanyName == supplierName && item.Product.ProductType == prodtype)
                {
                    if ((partName != null && item.Product.ProductName.Contains(partName)) || partName == null)
                    {

                        result.Add(Json(new { productName = item.Product.ProductName, price = item.Product.Price, type = item.Product.ProductType, supplierName = item.Supplier.CompanyName }));
                    }
                    else continue;

                }


            }

            return result;

        }

        [Route("products/searchBySupplier2")]
        [HttpPost]
        public List<JsonResult> searchBySupplier2(string address, string phone, string name)
        {
            bool flag = false;
            int id = (from s in _context.Suppliers
                      where s.Address == address
                      select s.ID).SingleOrDefault<int>();

            List<JsonResult> result = new List<JsonResult>();
            var query = from sup in _context.Suppliers
                        join prod in _context.Products on sup.ID equals prod.SupplierID
                        where prod.SupplierID == sup.ID
                        select new { Supplier = sup, Product = prod };


            foreach (var item in query)
            {

                if (item.Supplier.Address == address && item.Supplier.PhoneNumber == phone)
                {
                    if ((name != null && item.Product.ProductName.Contains(name)))
                    {
                     
                        result.Add(Json(new { comapnyName = item.Supplier.CompanyName,product=item.Product.ProductName, phone=item.Supplier.PhoneNumber,address = item.Supplier.Address, productName = item.Product.ProductName, exsist = true }));
                    }
                    else continue;

                }


            }

            

            return result;

        }

        [Route("products/searchByPriceAndType")]
        public List<Product> searchByPriceAndType(string productType, int min, int max)
        {


                if (max == 0)
            {
                max = 1000;
            }

            List<Product> products = new List<Product>();


            products = (from p in _context.Products
                        where (p.ProductType == productType) && (p.Price >= min && p.Price <= max)
                        select p).ToList<Product>();

            return products;

        }

        public Product getProductFromDB(int productID)
        {
            return (from p in _context.Products
                    where p.ID == productID
                    select p).SingleOrDefault<Product>();
        }

        // GET:: api/SubmitOrcer
        [Route("products/submitOrder")]
        [HttpGet]
        public IActionResult submitOrder(string jsonData)
        {
            double sum = 0;
            string username = "";
            // convert stringJSON to objectJSON:
            JObject json = JObject.Parse(jsonData);
            List<Product> productsCollection = new List<Product>();

            foreach (var item in json)
            {
                // if current item equals to username then dont try to drown deeper
                var isUserField = false;

                if (item.Key == "username")
                {
                    username = item.Value.ToString();
                    Console.WriteLine("Shopping cart belongs to: {0}, starting orderDetail ..", username);
                    isUserField = true;
                }

                if (isUserField == false)
                {
                    // parsing product details ..           
                    JObject product = JObject.Parse(item.Value.ToString());
                    string productID = item.Key;
                    string productName = "";
                    string productAmount = "";

                    foreach (var p in product)
                    {

                        if (p.Key == "productName")
                        {
                            productName = p.Value.ToString();
                        }

                        if (p.Key == "productAmount")
                        {
                            productAmount = p.Value.ToString();
                        }
                    }
                    // select spesific product:
                    Product prod = (from o in _context.Products
                                    where o.ProductName == productName
                                    select o).FirstOrDefault();
                    // casting
                    double sumOfProduct = prod.Price * Int32.Parse(productAmount);
                    sum += sumOfProduct;

                    Product myProduct = prod;
                    myProduct.Amount = Int32.Parse(productAmount);
                    productsCollection.Add(myProduct);
                }
            }

            User usr = (from usrs in _context.Users
                        where usrs.UserName == username
                        select usrs).FirstOrDefault();


            OrderDetails order = new OrderDetails();

            order.UserID = usr.ID;
            //order.Cart = new HashSet<UserProduct>();
            List<UserProduct> productToSave = new List<UserProduct>();

            foreach (Product p in productsCollection)
            {
                UserProduct product = new UserProduct();
                product.Amount = p.Amount;
                product.ProductName = p.ProductName;
                product.ProductType = p.ProductType;
                product.Price = p.Price;
                order.Cart.Add(product);
                productToSave.Add(product);
            }
            order.OrderTime = DateTime.Now;

            order.Total = sum;
            _context.OrderDetails.Add(order);

            _context.SaveChanges();
            var id = order.OrderID;

            OrderDetails orderDetails3 = _context.OrderDetails
              .Where(ood => ood.OrderID == id)
              .Include("Cart")
              .FirstOrDefault();

            JsonModel js = new JsonModel();
            js.sum = sum;
            js.userId = orderDetails3.UserID;
            js.username = username;
            js.products = productToSave;
            js.orderId = id;

            List<JsonModel> _data = new List<JsonModel>();
            _data.Add(js);
            string saveOrder = JsonConvert.SerializeObject(_data.ToArray());
            System.IO.File.WriteAllText(@"C:\Users\Eliran_Suisa\Desktop\WebApplications\2\StoreApp\wwwroot\orders\" + id.ToString() + ".json", saveOrder);

            return Redirect("/Index");
        }

        [Route("products/Search")]
        public ActionResult Search()
        {
            var supliers = from s in _context.Suppliers
                           select s;
            return View(supliers.ToList());
        }

        [Route("products/{productType}")]
        public ActionResult Products(String productType)
        { 
            Double Num;
            if (double.TryParse(productType, out Num))
            {
                var itemRange = (from m in _context.Products
                                 where m.Price <= Convert.ToDouble(productType)
                                 select m);
                return View(itemRange.ToList());
            }
            if (productType == "index")
            {
                var items = (from m in _context.Products
                             select m);
                return View(items.ToList());
            }
            var item = (from m in _context.Products
                        where m.ProductType == productType
                        select m);
            return View(item.ToList());


        }

        
        

        
    }
}
