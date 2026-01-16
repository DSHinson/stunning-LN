using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Models
{
    public record Product : EntityBase
    {
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
