﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Dollet.Core.Entities
{
    [Table("Currencies")]
    public class Currency : BaseEntity
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
