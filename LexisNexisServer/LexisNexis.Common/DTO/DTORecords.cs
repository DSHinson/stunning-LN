using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.DTO
{
    public sealed record CreateProductDto(string Name, string? Description, string? SKU, decimal Price, int Quantity);
}
