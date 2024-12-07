using Dollet.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dollet.Core.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; }
        public required int IndexOrder { get; set; }
        public CategoryType Type { get; set; } = CategoryType.Expense;
        public virtual ICollection<Expense> Expenses { get; set; }
        // Relația many-to-many
        public virtual ICollection<AccountCategory> AccountCategories { get; set; }
    }
}
