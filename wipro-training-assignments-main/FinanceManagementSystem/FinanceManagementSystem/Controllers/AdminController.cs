using FinanceManagementSystem.DAO;
using FinanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IFinanceRepository _repo;

        public AdminController(IFinanceRepository repo)
        {
            _repo = repo;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var users = _repo.GetAllUsers();
            return View(users);
        }

        public IActionResult UserDetails(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _repo.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var categories = _repo.GetCategoryExpenseSummaryForUser(id);
            var total = categories.Sum(c => c.TotalAmount);

            var vm = new AdminUserSummaryViewModel
            {
                User = user,
                Categories = categories,
                TotalAmount = total
            };

            return View(vm);
        }

        public IActionResult CategoryLeaders()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var leaders = _repo.GetTopSpendersPerCategory();
            return View(leaders);
        }
    }
}

