using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class GetProductByIdEventHandler : IQueryHandler<GetProductByIdEvent, Result<Product>>
    {
        IReadRepository<Product, int> _repo;

        public GetProductByIdEventHandler(IReadRepository<Product, int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Result<Product>> HandleAsync(GetProductByIdEvent query)
        {
            if (query is not { Id: > 0 })
            {
                return ResultHelpers.ToFailure<Product>("Invalid product Id");
            }

            Result<Product>? product = await _repo.GetByIdAsync(query.Id);

            if (product is null)
            {
                return ResultHelpers.ToFailure<Product>($"Product with ID {query.Id} not found");
            }

            return product;
        }

    }
}
