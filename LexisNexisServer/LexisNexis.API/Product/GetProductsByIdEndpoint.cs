using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Products
{
    public static class GetProductsByIdEndpoint
    {
        /// <summary>
        /// Maps GET /api/products/{id} endpoint.
        /// Returns a single product by its unique ID.
        /// </summary>
        public static WebApplication MapGetProductById(this WebApplication app)
        {
            app.MapGet("/api/products/{id:int}", async (EventPlayerService eventPlayerService, int id) =>
            {
                // Emit event to retrieve the product
                Result<Product> result = await eventPlayerService.EmitAsync(new GetProductByIdEvent { Id = id });

                return result.ToApiResponse();
            });

            return app;
        }
    }
}
