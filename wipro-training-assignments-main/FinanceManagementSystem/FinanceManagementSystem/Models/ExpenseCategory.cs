using System.ComponentModel.DataAnnotations;

namespace FinanceManagementSystem.Models
{

    public class ExpenseCategory
    {
        [Key]   // 👈 This is important
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
