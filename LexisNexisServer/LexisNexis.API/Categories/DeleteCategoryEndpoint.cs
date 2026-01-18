using LexisNexis.API.Helpers;
using LexisNexis.BLL.Categories;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.Result;

namespace LexisNexis.API.Categories
{
    public static class DeleteCategoryEndpoint
    {
        /// <summary>
        /// Deletes a product by id.
        /// </summary>
        public static WebApplication MapDeleteProduct(this WebApplication app)
        {
            app.MapDelete("/api/categories/{id:int}", async (int id, EventPlayerService eventPlayerService) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid category id");
                }

                Result result = await eventPlayerService.EmitAsync(new DeleteCategoryEvent { Id = id });

                return result.ToApiResponse($"Category with Id:{id} deleted");
            });

            return app;
        }


    }
}
