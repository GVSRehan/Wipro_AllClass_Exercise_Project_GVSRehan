using System.Collections.Generic;
using System.Linq;
using FinanceManagementSystem.Data;
using FinanceManagementSystem.Models;
using FinanceManagementSystem.Exceptions;

namespace FinanceManagementSystem.DAO
{
    public class FinanceRepositoryImpl : IFinanceRepository
    {
        private readonly FinanceDbContext _context;

        public FinanceRepositoryImpl(FinanceDbContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool CreateExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                throw new UserNotFoundException("User not found with id: " + userId);

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteExpense(int expenseId)
        {
            var exp = _context.Expenses.Find(expenseId);
            if (exp == null)
                throw new ExpenseNotFoundException("Expense not found with id: " + expenseId);

            _context.Expenses.Remove(exp);
            _context.SaveChanges();
            return true;
        }

        public List<Expense> GetAllExpenses(int userId)
        {
            return _context.Expenses
                .Where(e => e.UserId == userId)
                .ToList();
        }

        public List<Expense> GetExpensesByDateRange(int userId, DateTime? from, DateTime? to)
        {
            var query = _context.Expenses.AsQueryable();

            query = query.Where(e => e.UserId == userId);

            if (from.HasValue)
            {
                query = query.Where(e => e.Date >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(e => e.Date <= to.Value);
            }

            return query
                .OrderBy(e => e.Date)
                .ToList();
        }

        public List<ExpenseCategory> GetAllCategories()
        {
            return _context.ExpenseCategories
                .OrderBy(c => c.CategoryName)
                .ToList();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users
                .OrderBy(u => u.Username)
                .ToList();
        }

        public List<CategoryExpenseSummary> GetCategoryExpenseSummaryForUser(int userId)
        {
            var query =
                from e in _context.Expenses
                join c in _context.ExpenseCategories on e.CategoryId equals c.CategoryId
                where e.UserId == userId
                group new { e, c } by c.CategoryName into g
                orderby g.Key
                select new CategoryExpenseSummary
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(x => x.e.Amount)
                };

            return query.ToList();
        }

        public List<CategoryTopSpender> GetTopSpendersPerCategory()
        {
            // First query runs in SQL and returns sums per (Category, User)
            var perUserCategory = (
                from e in _context.Expenses
                join u in _context.Users on e.UserId equals u.UserId
                join c in _context.ExpenseCategories on e.CategoryId equals c.CategoryId
                group e by new { c.CategoryName, u.Username } into g
                select new
                {
                    g.Key.CategoryName,
                    g.Key.Username,
                    Total = g.Sum(x => x.Amount)
                }
            ).ToList(); // materialize, further grouping is in-memory

            // Now in memory: for each category pick top 3 users by total
            var topPerCategory = perUserCategory
                .GroupBy(x => x.CategoryName)
                .SelectMany(g => g
                    .OrderByDescending(x => x.Total)
                    .Take(3)
                    .Select(x => new CategoryTopSpender
                    {
                        CategoryName = x.CategoryName,
                        Username = x.Username,
                        TotalAmount = x.Total
                    }))
                .OrderBy(x => x.CategoryName)
                .ThenByDescending(x => x.TotalAmount)
                .ToList();

            return topPerCategory;
        }

        public bool UpdateExpense(int userId, Expense expense)
        {
            var existing = _context.Expenses
                .FirstOrDefault(e => e.ExpenseId == expense.ExpenseId && e.UserId == userId);

            if (existing == null)
                throw new ExpenseNotFoundException("Expense not found for update");

            existing.Amount = expense.Amount;
            existing.CategoryId = expense.CategoryId;
            existing.Date = expense.Date;
            existing.Description = expense.Description;

            _context.SaveChanges();
            return true;
        }
    }
}