using LexisNexis.Common.DTO;
using LexisNexis.DAL.Models;

namespace LexisNexis.DAL.Mappers
{
    public static class Mappers
    {
        public static ProductDto ToDto(this Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return new ProductDto(
                Id: product.Id,
                Name: product.Name,
                Description: product.Description,
                SKU: product.SKU,
                CategoryId: product.CategoryId,
                Price: product.Price,
                Quantity: product.Quantity,
                CreatedAt: product.CreatedAt,
                UpdatedAt: product.UpdatedAt
            );
        }
    }
}
