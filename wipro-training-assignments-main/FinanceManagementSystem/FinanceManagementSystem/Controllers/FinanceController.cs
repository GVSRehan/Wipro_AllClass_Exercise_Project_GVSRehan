using FinanceManagementSystem.DAO;
using FinanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceManagementSystem.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceRepository _repo;

        public FinanceController(IFinanceRepository repo)
        {
            _repo = repo;
        }

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = _repo.GetAllExpenses(userId.Value);
            return View(expenses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = new SelectList(_repo.GetAllCategories(), "CategoryId", "CategoryName");
            return View(new Expense { Date = DateTime.Today, UserId = userId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense expense)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            expense.UserId = userId.Value;

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_repo.GetAllCategories(), "CategoryId", "CategoryName");
                return View(expense);
            }

            _repo.CreateExpense(expense);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expense = _repo.GetAllExpenses(userId.Value)
                .FirstOrDefault(e => e.ExpenseId == id);

            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_repo.GetAllCategories(), "CategoryId", "CategoryName");
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Expense expense)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_repo.GetAllCategories(), "CategoryId", "CategoryName");
                return View(expense);
            }

            _repo.UpdateExpense(userId.Value, expense);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expense = _repo.GetAllExpenses(userId.Value)
                .FirstOrDefault(e => e.ExpenseId == id);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            _repo.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Report(DateTime? from, DateTime? to)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = _repo.GetExpensesByDateRange(userId.Value, from, to);
            ViewBag.From = from?.ToString("yyyy-MM-dd");
            ViewBag.To = to?.ToString("yyyy-MM-dd");
            return View(expenses);
        }

        /// <summary>
        /// API: returns expense report data for chart and table. Fetched from DB via backend.
        /// </summary>
        [HttpGet]
        public IActionResult ReportData(DateTime? from, DateTime? to)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Json(new { error = "Not authenticated" });
            }

            var expenses = _repo.GetExpensesByDateRange(userId.Value, from, to).OrderBy(e => e.Date).ToList();
            var total = expenses.Sum(e => e.Amount);

            var byDate = expenses
                .GroupBy(e => e.Date.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { label = g.Key.ToString("dd MMM yyyy"), amount = (double)g.Sum(e => e.Amount) })
                .ToList();

            var chartLabels = byDate.Select(x => x.label).ToList();
            var chartAmounts = byDate.Select(x => x.amount).ToList();

            var rows = expenses.Select(e => new
            {
                expenseId = e.ExpenseId,
                amount = e.Amount,
                categoryId = e.CategoryId,
                date = e.Date.ToString("yyyy-MM-dd"),
                description = e.Description ?? ""
            }).ToList();

            return Json(new
            {
                total = (double)total,
                chartLabels,
                chartAmounts,
                expenses = rows
            });
        }

        [HttpGet]
        public IActionResult Analysis(string period = "Monthly")
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var now = DateTime.Today;
            DateTime from;
            var labels = new List<string>();
            var expenseData = new List<decimal>();

            switch (period?.ToLowerInvariant())
            {
                case "daily":
                    from = now.AddDays(-6);
                    for (var d = from; d <= now; d = d.AddDays(1))
                    {
                        labels.Add(d.ToString("ddd"));
                        var dayExpenses = _repo.GetExpensesByDateRange(userId.Value, d, d.AddDays(1).AddTicks(-1));
                        expenseData.Add(dayExpenses.Sum(e => e.Amount));
                    }
                    break;
                case "weekly":
                    from = now.AddDays(-27);
                    for (var i = 0; i < 4; i++)
                    {
                        var weekStart = from.AddDays(i * 7);
                        var weekEnd = weekStart.AddDays(6);
                        labels.Add($"{i + 1}st Week");
                        var weekExpenses = _repo.GetExpensesByDateRange(userId.Value, weekStart, weekEnd);
                        expenseData.Add(weekExpenses.Sum(e => e.Amount));
                    }
                    break;
                case "year":
                    from = now.AddYears(-4);
                    for (var y = 0; y < 5; y++)
                    {
                        var yearStart = from.AddYears(y);
                        var yearEnd = yearStart.AddYears(1).AddDays(-1);
                        labels.Add(yearStart.Year.ToString());
                        var yearExpenses = _repo.GetExpensesByDateRange(userId.Value, yearStart, yearEnd);
                        expenseData.Add(yearExpenses.Sum(e => e.Amount));
                    }
                    break;
                default: // Monthly
                    period = "Monthly";
                    from = now.AddMonths(-5);
                    for (var m = 0; m < 6; m++)
                    {
                        var monthStart = from.AddMonths(m);
                        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                        labels.Add(monthStart.ToString("MMM"));
                        var monthExpenses = _repo.GetExpensesByDateRange(userId.Value, monthStart, monthEnd);
                        expenseData.Add(monthExpenses.Sum(e => e.Amount));
                    }
                    break;
            }

            var allExpenses = _repo.GetExpensesByDateRange(userId.Value, from, now);
            var periodTotal = allExpenses.Sum(e => e.Amount);
            var allTimeExpenses = _repo.GetAllExpenses(userId.Value);
            var totalSpent = allTimeExpenses.Sum(e => e.Amount);
            const decimal targetDefault = 20000m;
            var percentOfTarget = targetDefault > 0 ? (double)(totalSpent / targetDefault * 100) : 0;
            if (percentOfTarget > 100) percentOfTarget = 100;

            ViewBag.ChartLabels = labels;
            ViewBag.ChartExpenseData = expenseData;
            ViewBag.Period = period;
            ViewBag.TotalBalance = 0m;
            ViewBag.TotalExpense = totalSpent;
            ViewBag.PeriodExpense = periodTotal;
            ViewBag.Target = targetDefault;
            ViewBag.PercentOfTarget = (int)percentOfTarget;

            return View();
        }
    }
}
