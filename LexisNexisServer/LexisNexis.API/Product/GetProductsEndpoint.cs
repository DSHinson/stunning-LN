using LexisNexis.BLL.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Mappers;

namespace LexisNexis.API.Products
{
    public static class GetProductsEndpoint
    {
        /// <summary>
        /// Maps the GET /api/products endpoint with pagination, category filtering, and search by name.
        /// </summary>
        public static WebApplication MapGetProducts(this WebApplication app)
        {
            app.MapGet("/api/products", async (EventPlayerService eventPlayerService, int page = 1, int pageSize = 10,int? category = null, string? search = null) =>
            {
                Result<IEnumerable<Product>> result = await eventPlayerService.EmitAsync(new GetProductsEvent() {
                    Search = search,
                    Category = category,
                    Page = page,
                    PageSize = pageSize
                });

                if (result is Result<IEnumerable<Product>>.Failure failure)
                {
                    return Results.Problem(failure.FailureMessage);
                }

                IEnumerable<Product> data = (result as Result<IEnumerable<Product>>.Success)!.Data;

                return Results.Ok(new ProductPageResponse( page, pageSize,data.Count(), data.Select(x => x.ToDto()).ToList()));
            });

            return app;
        }
    }
}
