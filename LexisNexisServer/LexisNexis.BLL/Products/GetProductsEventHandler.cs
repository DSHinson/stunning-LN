using LexisNexis.Common.Cache;
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
        ICacheService _cacheService;

        public GetProductsEventHandler(IReadRepository<Product, int> repo, ICacheService cacheService)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<Result<IEnumerable<Product>>> HandleAsync(GetProductsEvent query)
        {
            Guid cacheKey = _cacheService.CreateKey(nameof(GetProductsEvent), query.Page, query.PageSize, query.Search, query.Category);

            CacheContainer<IEnumerable<Product>> cacheResult = _cacheService.GetOrCreateCacheContainer<IEnumerable<Product>>(cacheKey);

            if (cacheResult.Data is not null)
            {
                return ResultHelpers.ToResult(cacheResult.Data);
            }

            await cacheResult.LockAsync();

            try
            {
                // Double check after acquiring the lock to prevent stampeding
                if (cacheResult.Data is not null)
                {
                    return ResultHelpers.ToResult(cacheResult.Data);
                }


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

                var result = await _repo.GetAllAsync(predicate);

                //if successful update the cache container
                if (result is Result<IEnumerable<Product>>.Success success)
                {
                    cacheResult.Data = success.Data;
                    _cacheService.Set(cacheKey, cacheResult, TimeSpan.FromSeconds(30));
                }

                return result;
            }
            finally
            {
                cacheResult?.Release();
            }
        }
    }
}
