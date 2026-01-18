using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;

namespace LexisNexis.API.Products
{
    public static class UpdateProductEndPoint
    {
        /// <summary>
        /// Maps PUT /api/products/{id} to update an existing product.
        /// </summary>
        public static WebApplication MapUpdateProduct(this WebApplication app)
        {
            app.MapPut("/api/products/{id:int}", async (EventPlayerService eventPlayerService, int id, UpdateProductDto updateProduct) =>
            {
                if (updateProduct is null)
                {
                    return Results.BadRequest("Invalid payload");
                }

                // Emit update event
                var result = await eventPlayerService.EmitAsync(new UpdateProductEvent
                {
                    Id = id,
                    Name = updateProduct.Name,
                    Description = updateProduct.Description,
                    SKU = updateProduct.SKU,
                    Price = updateProduct.Price,
                    Quantity = updateProduct.Quantity,
                    CategoryId = updateProduct.CategoryId ?? 0
                });

                return result.ToApiResponse();
            });

            return app;
        }

    }
}
