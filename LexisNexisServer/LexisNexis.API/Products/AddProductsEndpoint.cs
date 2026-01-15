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
            app.MapPost("/api/products", async (EventPlayerService eventPlayerService, CreateProductDto ProductData , HttpContext httpContext) =>
            {
                // Manual model binding
                //CreateProductDto? dto = await httpContext.Request.ReadFromJsonAsync<CreateProductDto>();

                Result<Product> result = await eventPlayerService.EmitAsync(new CreateProductEvent
                {
                        Name = ProductData.Name,
                        Description = ProductData.Description,
                        SKU = ProductData.SKU,
                        Price = ProductData.Price,
                        Quantity = ProductData.Quantity,
                    });

                if (result is Result<Product>.Failure failure)
                {
                    return Results.Problem(failure.FailureMessage);
                }

                Product product = ((Result<Product>.Success)result).Data;

                return Results.Created($"/api/products/{product.Id}", product);
            });

            return app;
        }

    }
}
