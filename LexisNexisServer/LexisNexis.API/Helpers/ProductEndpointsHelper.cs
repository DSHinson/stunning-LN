using LexisNexis.API.Products;

namespace LexisNexis.API.Helpers
{
    public static class ProductEndpointsHelper
    {
        public static WebApplication MapProductsApi(this WebApplication app)
        {
            app.MapGetProducts();
            app.MapPostProduct();
            app.MapGetProductById();
            app.MapUpdateProduct();
            app.MapDeleteProduct();

            return app;
        }
    }

}
