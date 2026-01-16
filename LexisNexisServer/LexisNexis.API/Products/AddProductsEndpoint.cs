using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Products
{
    public  static class AddProductsEndpoint
    {
        /// <summary>
        /// Maps the POST /api/products endpoint for creating a new product.
        /// </summary>
        public static WebApplication MapPostProduct(this WebApplication app)
        {
            app.MapPost("/api/products", async (CreateProductDto ProductData , HttpContext httpContext, EventPlayerService eventPlayerService) =>
            {
                Result<Product> result = await eventPlayerService.EmitAsync(new CreateProductEvent
                {
                        Name = ProductData.Name,
                        Description = ProductData.Description,
                        SKU = ProductData.SKU,
                        Price = ProductData.Price,
                        Quantity = ProductData.Quantity,
                        CategoryId = ProductData.CategoryId ?? 0
                });

                return result.ToApiResponse();
            });

            return app;
        }

    }
}
