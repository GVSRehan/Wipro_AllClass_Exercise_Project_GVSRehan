using Microsoft.EntityFrameworkCore;
using TransactionDemo.Models;

namespace TransactionDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}