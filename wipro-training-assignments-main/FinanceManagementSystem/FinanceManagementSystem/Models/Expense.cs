using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinanceManagementSystem.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        public int UserId { get; set; }
        [ValidateNever]
        public User? User { get; set; }

        [Column(TypeName = "decimal(18,2)")]  // 👈 THIS FIXES THE WARNING
        public decimal Amount { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public ExpenseCategory? Category { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
