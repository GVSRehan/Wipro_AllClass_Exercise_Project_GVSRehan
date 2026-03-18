using System.ComponentModel.DataAnnotations;

namespace FinanceManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
