using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Products
{
    public  static class DeleteProductsEndpoint
    {
        /// <summary>
        /// Deletes a product by id.
        /// </summary>
        public static WebApplication MapDeleteProduct(this WebApplication app)
        {
            app.MapDelete("/api/products/{id:int}", async (
                int id,
                EventPlayerService eventPlayerService) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid product id");
                }

                Result result = await eventPlayerService.EmitAsync(new DeleteProductEvent { Id = id });

                return result.ToApiResponse($"Product with Id:{id} deleted");
            });

            return app;
        }


    }
}
