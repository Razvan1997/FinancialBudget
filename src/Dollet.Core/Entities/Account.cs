using Dollet.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dollet.Core.Entities
{
    [Table("Accounts")]
    public class Account : BaseEntity
    {
        public required decimal Amount { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; }
        public required string Currency { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDefault { get; private set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        // Relație cu User
        public required int UserId { get; set; } // Cheia străină
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }

        public required string Username { get; set; }
        public required string Password { get; set; }
        public UserType Role { get; set; }

        // Relația many-to-many
        public virtual ICollection<AccountCategory> AccountCategories { get; set; }

        public void SetAsDefault() => IsDefault = true;
        public void UnsetAsDefault() => IsDefault = false;
    }
}
