using System.ComponentModel.DataAnnotations.Schema;

namespace Dollet.Core.Entities
{
    [Table("CurrencyValue")]
    public class CurrencyValue : BaseEntity
    {
        public required string CodeFrom { get; set; }
        public required string CodeTo { get; set; }
        public required decimal Value { get; set; }
    }
}
