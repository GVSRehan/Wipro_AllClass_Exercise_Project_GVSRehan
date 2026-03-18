using FinanceManagementSystem.Models;
using System.Collections.Generic;

namespace FinanceManagementSystem.DAO
{
    public interface IFinanceRepository
    {
        bool CreateUser(User user);
        User? GetUserById(int userId);
        User? GetUserByCredentials(string username, string password);

        bool CreateExpense(Expense expense);
        bool DeleteUser(int userId);
        bool DeleteExpense(int expenseId);
        List<Expense> GetAllExpenses(int userId);
        List<Expense> GetExpensesByDateRange(int userId, DateTime? from, DateTime? to);
        List<ExpenseCategory> GetAllCategories();
        bool UpdateExpense(int userId, Expense expense);
        List<User> GetAllUsers();
        List<CategoryExpenseSummary> GetCategoryExpenseSummaryForUser(int userId);
        List<CategoryTopSpender> GetTopSpendersPerCategory();
    }
}
