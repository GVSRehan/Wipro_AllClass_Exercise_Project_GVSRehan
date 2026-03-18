using System.Collections.Generic;

namespace FinanceManagementSystem.Models
{
    public class AdminUserSummaryViewModel
    {
        public User User { get; set; } = null!;
        public List<CategoryExpenseSummary> Categories { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}

