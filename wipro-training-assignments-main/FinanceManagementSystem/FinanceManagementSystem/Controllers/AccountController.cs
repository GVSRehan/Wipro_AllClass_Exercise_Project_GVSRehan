using FinanceManagementSystem.DAO;
using FinanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFinanceRepository _repo;

        public AccountController(IFinanceRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _repo.CreateUser(user);
            TempData["Message"] = "User registered successfully. Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            // Simple admin login (not stored in DB)
            if (username == "Admin" && password == "Admin@123")
            {
                HttpContext.Session.SetString("Username", "Admin");
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Admin");
            }

            var user = _repo.GetUserByCredentials(username, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.Remove("IsAdmin");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

