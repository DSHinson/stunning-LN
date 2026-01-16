using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Categories
{
    public static class GetCategoryEndpoint
    {
        /// <summary>
        /// Maps the GET /api/categories endpoint
        /// </summary>
        public static WebApplication MapGetCategories(this WebApplication app)
        {
            app.MapGet("/api/categories", async (EventPlayerService eventPlayerService) =>
            {
                Result<IEnumerable<Category>> result = await eventPlayerService.EmitAsync(new GetCategoriesEvent());

                return result.ToApiResponse();
            });

            return app;
        }
    }
}
