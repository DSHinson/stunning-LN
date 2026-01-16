using LexisNexis.API.Helpers;
using LexisNexis.BLL.Categories;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.API.Categories
{
    public  static class AddCategoryEndpoint
    {
        /// <summary>
        /// Maps the POST /api/category endpoint for creating a new category.
        /// </summary>
        public static WebApplication MapPostCategories(this WebApplication app)
        {
            app.MapPost("/api/categories", async (CreateCategoryDto CategoryData , EventPlayerService eventPlayerService) =>
            {
                Result<Category> result = await eventPlayerService.EmitAsync(new CreateCategoryEvent
                {
                        Name = CategoryData.Name,
                        Description = CategoryData.Description,
                        ParentCategoryId = CategoryData.ParentId
                });

                return result.ToApiResponse();
            });

            return app;
        }

    }
}
