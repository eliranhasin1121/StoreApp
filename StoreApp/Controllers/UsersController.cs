using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class UsersController : Controller
    {
        private StoreContext _context;

        public UsersController(StoreContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,UserName,Password,Mail,BirthDay")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string firstname, string lastname, string email, DateTime bday, string username, string password)
        {
            List <User> users = (from user in _context.Users select user).ToList();

            for (int i = 0; i< users.Count; i++)
            {
                if (users[i].UserName == username)
                {
                    ViewBag.Message = users[i].UserName + "==" + username + i.ToString() + "exist!";
                    return View();
                }
            }
            if (firstname == "" || lastname == "" || username == "" || password == "")
            {
                ViewBag.Message = "invalid input";
                return View();
            }
            User registerUser = new User();
            registerUser.FirstName = firstname;
            registerUser.LastName = lastname;
            registerUser.UserName = username;
            registerUser.Password = password;

            _context.Users.Add(registerUser);
            await _context.SaveChangesAsync();
            ViewBag.Message += "user created.";
            return Redirect("/users/login");
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var authResult = (from user in _context.Users where (user.UserName == username && user.Password == password) select user).ToList();
            bool existUser = authResult.ToList().Any(user => user.UserName == username);
            bool existPass = authResult.ToList().Any(user => user.Password == password);
            if (existUser && existPass)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.Message = "invalid user or password!";
                return View();
            } 
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,UserName,Password,Mail,BirthDay")] User user)
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
    }
}
