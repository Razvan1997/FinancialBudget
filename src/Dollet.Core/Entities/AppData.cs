using System.ComponentModel.DataAnnotations.Schema;

namespace Dollet.Core.Entities
{
    [Table("AppData")]
    public class AppData : BaseEntity
    {
        public DateTime LastRun { get; set; }
    }
}