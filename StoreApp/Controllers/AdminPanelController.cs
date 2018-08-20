using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
   
    public class AdminPanelController : Controller
    {
        private class Item
        {
            public string name { set; get; }
            public int count { set; get; }
        }
        private readonly StoreContext _context;

       

        public AdminPanelController(StoreContext context)
        {
            _context = context;
        }

        public double calcCart(List<Product> cart) {

            double sum = 0;

            foreach (Product p in cart) {
                sum += p.Price;
            }

            return sum;
        }

        public double calcEarns() {
            double totalEarns=0;
            var order = (from ord in _context.OrderDetails 
                         select ord);


            foreach (OrderDetails o in order)
            {
                totalEarns += o.Total;
            }
                   
            return totalEarns;
        }

        public IActionResult Index()
        {
            Dictionary<int, double> IdSellsHashMap = new Dictionary<int, double>();

            foreach (OrderDetails o in _context.OrderDetails)
            {
                if (IdSellsHashMap.ContainsKey(o.UserID))
                {
                    if (o.Total > IdSellsHashMap[o.UserID])
                    {
                        IdSellsHashMap[o.UserID] = o.Total;
                    }
                
                } else
                {
                    IdSellsHashMap[o.UserID] = o.Total;
                }
            }


            //remember! topProducts is KeyValuePairs List

            List<User> newUsers = new List<User>();
            var pairsTotalId = IdSellsHashMap.OrderBy(pair => pair.Value).Take(5).ToList();
            pairsTotalId.OrderByDescending(pair => pair.Value);
            foreach(KeyValuePair<int, double> Kv in pairsTotalId)
            {
                User u = (from usrs in _context.Users
                          where usrs.ID == Kv.Key
                          select usrs).SingleOrDefault();

                newUsers.Add(u);
            }

            ViewBag.totalIds = IdSellsHashMap;
            ViewBag.users = newUsers;
            ViewBag.totalEarns = calcEarns();

            var orders = (from o in _context.OrderDetails
                         select o);
            ViewBag.ordersNum = orders.Count();

            var users = (from u in _context.Users
                            select u);
            ViewBag.usersNum = users.Count();



            var products = (from p in _context.Products
                            where p.Amount == 0
                            select p);

            ViewBag.missingProducts = products;

            var type = from p in _context.Products
                        group p by p.ProductType into g
                        select new
                        {
                            name = g.Key,
                            count = g.Count()
                        };
            string str = "";
                foreach (var item in type)
            {
                str += "{name:'"+item.name+"',y:"+item.count+"},";
            }
           if (str.Length > 0)
            {
                str = str.Remove(str.Length-1);
            }
            ViewBag.productType = str;
            return View();
        }

        [Route("AdminPanel/Products")]
        public async Task<IActionResult> Products()
        {
           return View(await _context.Products.ToListAsync());
        }
 
        //Get: AdminPanel/createProduct
        public IActionResult CreateProduct()
        {
            return View();
        }


        // POST: AdminPanel/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("ID,ProductName,Amount,ProductType,Price,ImageURL,Description,SupplierID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: AdmonPanel/Products/Details/5
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST:  AdminPanel/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, [Bind("ID,ProductName,Amount,ProductType,Price,ImageURL,Description,SupplierID")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string AuthUser(string user)
        {
            JObject json = JObject.Parse(user);
            string username = "";
            string password = "";
            foreach (var field in json)
            {
                if (field.Key == "username")
                {
                    username = field.Value.ToString();
                }
                if (field.Key == "password")
                {
                    password = field.Value.ToString();
                }
            }
            User userVerified = (from users in _context.Users
                                 where users.UserName == username && users.Password == password && users.IsAdmin == true
                                 select users).FirstOrDefault();
            if (userVerified != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        // GET:  AdminPanel/Products/DeleteProduct/5
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST:  AdminPanel/Products/DeleteProduct/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        // Get: AdminPanel/Users
        [Route("AdminPanel/Users")]
        public async Task<IActionResult> Users()
        {
            return View(await _context.Users.ToListAsync());
        }

        // Get: AdminPanel/CreateUser
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: AdminPanel/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("ID,FirstName,LastName,UserName,Password,Mail,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users", new { ID = user.ID });

            }
            return View(user);
        }

        // GET: AdminPanel/Users/UserDetails/5
        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: AdminPanel/Users/EditUser/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: AdmonPanel/Users/EditUser/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("ID,FirstName,LastName,UserName,Password,Mail,BirthDay")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: AdmonPanel/Users/DeleteUser/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        // POST: AdminPanel/Users/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.ID == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }

        // Get: AdminPanel/CreateSupplier
        public IActionResult CreateSupplier()
        {
            return View();
        }

        [Route("AdminPanel/Suppliers")]
        public async Task<IActionResult> Suppliers()
        {
            return View(await _context.Suppliers.ToListAsync());
        }


        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSupplier([Bind("ID,CompanyName,PhoneNumber,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }


        // GET: AdminPanel/Suppliers/EditSupplier/5
        public async Task<IActionResult> EditSupplier(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.SingleOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: AdmonPanel/Suppliers/EditSupplier/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupplier(int id, [Bind("ID,CompanyName,PhoneNumber,Address")] Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: AdminPanel/Suppliers/DeleteSupplier/5
        public async Task<IActionResult> DeleteSupplier(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: adminPanel/Suppliers/DeleteSupplier/5
        [HttpPost, ActionName("DeleteSupplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.ID == id);
        }

        // GET: AdminPanel/Suppliers/SupplierDetails/5
        public async Task<IActionResult> SupplierDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }


        [HttpPost]
        public List<JsonResult> searchByUser(int userId, DateTime begin, DateTime end)
        {

            var query = from ord in _context.OrderDetails
                        join usr in _context.Users on ord.UserID equals usr.ID
                        where usr.ID == ord.UserID
                        select new { User = usr, OrderDetails = ord };

            int beg = begin.DayOfYear;
            int e = end.DayOfYear;
            List<JsonResult> result = new List<JsonResult>();

            foreach (var item in query)
            {
                if (item.OrderDetails.UserID.Equals(userId))
                {
                    if (item.OrderDetails.OrderTime.DayOfYear >= beg && item.OrderDetails.OrderTime.DayOfYear <= e)
                    {

                        result.Add(Json(new { orderID = item.OrderDetails.OrderID, firsName = item.User.FirstName, lastName = item.User.LastName, date = item.OrderDetails.OrderTime, total = item.OrderDetails.Total, numOfproducts = item.OrderDetails.Cart.Count() }));

                    }
                }
            }





            return result;

        }
        [HttpPost]
        public JsonResult userSearch(int usrId)
        {
            User query = (from u in _context.Users
                          where u.ID == usrId
                          select u).SingleOrDefault<User>();

           return (Json(new {userID=query.ID,userName=query.UserName,firstName=query.FirstName
              ,lastName=query.LastName ,email=query.Mail, }));

           
        }

        [HttpPost]
        public JsonResult prodSearch(int prodId)
        {
            Product query = (from p in _context.Products
                          where p.ID == prodId
                          select p).SingleOrDefault<Product>();

            return (Json(new
            {

                prodID = query.ID,
                productName = query.ProductName,
                productType = query.ProductType,
                productAmount = query.Amount,
                productPrice = query.Price,
                productDescription = query.Description,
                supplierId = query.SupplierID
                
            }));


        }

        [HttpPost]
        public JsonResult supplierSearch(int supId)
        {
            Supplier query = (from s in _context.Suppliers
                             where s.ID == supId
                             select s).SingleOrDefault<Supplier>();

            return (Json(new
            {

                id = query.ID,
                companyName = query.CompanyName,
                address=query.Address,
                phoneNumber = query.PhoneNumber
               

            }));


        }
    }
 

   
}
