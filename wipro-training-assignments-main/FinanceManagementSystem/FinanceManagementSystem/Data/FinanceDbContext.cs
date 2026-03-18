using FinanceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagementSystem.Data
{

    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    }
}