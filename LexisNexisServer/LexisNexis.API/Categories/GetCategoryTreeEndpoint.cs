using LexisNexis.API.Helpers;
using LexisNexis.BLL.Categories;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Categories
{
    public static class GetCategoryTreeEndpoint
    {
        /// <summary>
        /// Maps the GET /api/categories endpoint
        /// </summary>
        public static WebApplication MapGetCategoriesTree(this WebApplication app)
        {
            app.MapGet("/api/categories/tree", async (EventPlayerService eventPlayerService) =>
            {
                Result<IReadOnlyList<CategoryNodeDto>> result = await eventPlayerService.EmitAsync(new GetCategoriesTreeEvent());

                return result.ToApiResponse();
            });

            return app;
        }
    }
}
