using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.DTO
{
    public sealed record CreateProductDto(string Name, string? Description, string? SKU, int? CategoryId, decimal Price, int Quantity);
    public sealed record UpdateProductDto(int productId,string Name,string? Description, string? SKU, int? CategoryId ,decimal Price,int Quantity);
    public sealed record CategoryNodeDto(int Id,string Name,string Description,IReadOnlyList<CategoryNodeDto> Children);
    public sealed record CreateCategoryDto(string Name, string Description, int? ParentId);
}
