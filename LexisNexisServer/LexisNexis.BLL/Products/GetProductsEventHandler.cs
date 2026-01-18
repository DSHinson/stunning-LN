using LexisNexis.BLL.SearchEngine;
using LexisNexis.Common.Cache;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Filters;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using System.Linq;
using System.Linq.Expressions;

namespace LexisNexis.BLL.Products
{
    public class GetProductsEventHandler : IQueryHandler<GetProductsEvent, Result<IEnumerable<Product>>>
    {
        private readonly ISearchEngine<Category, int> _categorySearchEngine;
        private readonly ISearchEngine<Product, int> _productSearchEngine;
        private readonly IReadRepository<Category, int> _categoryRepository;
        private readonly ICacheService _cacheService;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromSeconds(1);

        public GetProductsEventHandler(ICacheService cacheService, ISearchEngine<Category, int> categorySearchEngine, ISearchEngine<Product, int> productSearchEngine, IReadRepository<Category, int> categoryRepository)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _categorySearchEngine = categorySearchEngine ?? throw new ArgumentNullException(nameof(categorySearchEngine));
            _productSearchEngine = productSearchEngine ?? throw new ArgumentNullException(nameof(productSearchEngine));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
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
                if (cacheResult.Data is not null)
                {
                    return ResultHelpers.ToResult(cacheResult.Data);
                }

                IEnumerable<Product> productCandidates = Enumerable.Empty<Product>();

                // Step 1: Search products directly by query
                if (!string.IsNullOrWhiteSpace(query.Search))
                {
                    var productSearchResult = await _productSearchEngine.Search(query.Search);
                    if (productSearchResult is Result<IEnumerable<Product>>.Failure failure)
                    {
                        return ResultHelpers.ToFailure<IEnumerable<Product>>(failure.FailureMessage);
                    }

                    productCandidates = ((Result<IEnumerable<Product>>.Success)productSearchResult).Data;
                }

                // Step 2: Search categories by query
                IEnumerable<int> matchedCategoryIds = Enumerable.Empty<int>();
                if (!string.IsNullOrWhiteSpace(query.Search))
                {
                    var categorySearchResult = await _categorySearchEngine.Search(query.Search);
                    if (categorySearchResult is Result<IEnumerable<Category>>.Failure failure)
                    {
                        return ResultHelpers.ToFailure<IEnumerable<Product>>(failure.FailureMessage);
                    }

                    matchedCategoryIds = ((Result<IEnumerable<Category>>.Success)categorySearchResult).Data
                        .Select(c => c.Id);
                }

                // Step 3: Build predicate for products in matched categories or specified category filter
                Expression<Func<Product, bool>>? predicate = null;

                // If a specific category is supplied, filter **strictly by that category**
                if (query.Category is not null)
                {
                    // Get all child categories recursively
                    List<Category> recursiveCategoryList = await GetChildCategoriesRecursiveAsync(query.Category.Value);

                    if (recursiveCategoryList.Count > 0)
                    {
                        // Build a list of IDs including the parent category itself
                        var categoryIds = recursiveCategoryList.Select(x => x.Id).Append(query.Category.Value).ToList();

                        // Create expression filter
                        Expression<Func<Product, bool>> categoryFilter = p => categoryIds.Contains(p.CategoryId);
                        predicate = categoryFilter;
                    }
                    else
                    {
                        // No children, just filter by this category
                        Expression<Func<Product, bool>> categoryFilter = p => p.CategoryId == query.Category.Value;
                        predicate = categoryFilter;
                    }
                }

                // If no specific category was supplied, but category search found matches
                if (query.Category is null && matchedCategoryIds.Any())
                {
                    Expression<Func<Product, bool>> categorySearchFilter = p => matchedCategoryIds.Contains(p.CategoryId);

                    if (predicate == null)
                    {
                        predicate = categorySearchFilter;
                    }
                    else
                    {
                        predicate = predicate.And(categorySearchFilter); // Use AND just in case
                    }
                }


                // Step 4: Apply repository search engine
                var repoResult = await _productSearchEngine.Search(query.Search);
                if (repoResult is Result<IEnumerable<Product>>.Failure repoFailure)
                {
                    return ResultHelpers.ToFailure<IEnumerable<Product>>(repoFailure.FailureMessage);
                }

                IEnumerable<Product> finalProducts = ((Result<IEnumerable<Product>>.Success)repoResult).Data;

                // Step 5: If a predicate exists, filter products by it
                if (predicate != null)
                {
                    finalProducts = finalProducts.AsQueryable().Where(predicate);
                }

                //Step 6: Apply pagination
                finalProducts = finalProducts.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList();

                // Step 6: Cache the results
                cacheResult.Data = finalProducts;
                _cacheService.Set(cacheKey, cacheResult, _cacheDuration);

                return ResultHelpers.ToResult(finalProducts);
            }
            finally
            {
                cacheResult.Release();
            }
        }

        /// <summary>
        /// Recursively gets categories up to a max depth.
        /// </summary>
        /// <param name="parentId">The parent category to start from.</param>
        /// <param name="currentDepth">Current recursion depth (default 0).</param>
        /// <param name="maxDepth">Maximum recursion depth (default 3).</param>
        private async Task<List<Category>> GetChildCategoriesRecursiveAsync(int parentId, int currentDepth = 0, int maxDepth = 3)
        {
            if (currentDepth >= maxDepth)
            {
                return new List<Category>();
            }

            // Query children of the current category
            Result<IEnumerable<Category>> childrenResult = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == parentId);

            if (childrenResult is Result<IEnumerable<Category>>.Failure)
            {
                return new List<Category>();
            }

            var children = ((Result<IEnumerable<Category>>.Success)childrenResult).Data.ToList();
            var allDescendants = new List<Category>(children);

            // Recurse for each child
            foreach (var child in children)
            {
                var subChildren = await GetChildCategoriesRecursiveAsync(child.Id, currentDepth + 1, maxDepth);
                allDescendants.AddRange(subChildren);
            }

            return allDescendants;
        }
    }
}
