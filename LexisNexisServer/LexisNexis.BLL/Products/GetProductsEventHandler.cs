using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Filters;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using System.Linq.Expressions;

namespace LexisNexis.BLL.Products
{
    public class GetProductsEventHandler : IQueryHandler<GetProductsEvent, Result<IEnumerable<Product>>>
    {
        IReadRepository<Product, int> _repo;

        public GetProductsEventHandler(IReadRepository<Product, int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Result<IEnumerable<Product>>> HandleAsync(GetProductsEvent query)
        {
            Expression<Func<Product, bool>>? predicate = null;

            // Filter by category
            if (query.Category is not null)
            {
                Expression<Func<Product, bool>> categoryFilter = p => p.CategoryId == query.Category;

                predicate = predicate == null ? categoryFilter : predicate.And(categoryFilter);
            }

            // Search by name
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                Expression<Func<Product, bool>> searchFilter = p => p.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase);

                predicate = predicate == null ? searchFilter : predicate.And(searchFilter);
            }

            return await _repo.GetAllAsync(predicate);
        }
    }
}
