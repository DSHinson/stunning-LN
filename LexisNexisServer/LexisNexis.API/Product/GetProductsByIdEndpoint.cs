using LexisNexis.API.Helpers;
using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using System.Text.Json;

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
            app.MapGet("/api/products/{id:int}", async (EventPlayerService eventPlayerService, int id, HttpContext context) =>
            {
                // Emit event to retrieve the product
                Result<Product> result = await eventPlayerService.EmitAsync(new GetProductByIdEvent { Id = id });

                if (result is Result<Product>.Failure failure)
                {
                    return Results.NotFound(new { message = failure.FailureMessage });
                }

                Product product = ((Result<Product>.Success)result).Data;

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                options.Converters.Add(new ProductJsonConverter());

                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, product, options);

                return Results.Empty;
            });

            return app;
        }
    }
}
