using Microsoft.AspNetCore.Mvc;
using FinanceManagementSystem.DAO;

namespace FinanceManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinanceRepository _repo;

        public HomeController(IFinanceRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}