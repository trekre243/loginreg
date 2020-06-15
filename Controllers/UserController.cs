using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using LoginReg.Models;

namespace LoginReg.Controllers
{
    public class UserController : Controller
    {   
        private MyContext dbContext;

        public UserController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Register()
        {
            if(HttpContext.Session.GetString("FirstName") != null)
            {
                return RedirectToAction("Success");
            }
            return View("Register");
        }
        
        [HttpPost("user")]
        public IActionResult Create(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetString("LastName", newUser.LastName);
                HttpContext.Session.SetString("Email", newUser.Email);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Register");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            if(HttpContext.Session.GetString("FirstName") == null)
            {
                return RedirectToAction("Register");
            }
            return View("Success");
        }

        [HttpGet("login")]
        public IActionResult LoginForm()
        {
            if(HttpContext.Session.GetString("FirstName") != null)
            {
                return RedirectToAction("Success");
            }
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }

                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Login");
                }

                HttpContext.Session.SetString("FirstName", userInDb.FirstName);
                HttpContext.Session.SetString("LastName", userInDb.LastName);
                HttpContext.Session.SetString("Email", userInDb.Email);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }

    }
}
