using LexisNexis.API.Categories;

namespace LexisNexis.API.Helpers
{
    public static class CategoryEndpointsHelper
    {
        public static WebApplication MapCategoryApi(this WebApplication app)
        {
            app.MapGetCategories();
            app.MapPostCategories();
            app.MapGetCategoriesTree();
            app.MapDeleteProduct();

            return app;
        }
    }

}
