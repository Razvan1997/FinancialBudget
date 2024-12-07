using Dollet.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dollet.Core.Entities
{
    [Table("Users")]
    public class Users : BaseEntity
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public UserType Role { get; set; }
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
