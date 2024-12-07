using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.Core.Entities
{
    [Table("AccountCategories")]
    public class AccountCategory
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Budget { get; set; } = 0; // Valoare implicită 0
    }
}
