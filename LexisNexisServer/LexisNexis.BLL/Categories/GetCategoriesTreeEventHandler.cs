using LexisNexis.BLL.Categories;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class GetCategoriesTreeEventHandler : IQueryHandler<GetCategoriesTreeEvent, Result<IReadOnlyList<CategoryNodeDto>>>
    {
        IReadRepository<Category, int> _repo;

        public GetCategoriesTreeEventHandler(IReadRepository<Category, int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Result<IReadOnlyList<CategoryNodeDto>>> HandleAsync(GetCategoriesTreeEvent query)
        {
            // Retrieve categories from the repository
            Result<IEnumerable<Category>> result = await _repo.GetAllAsync();

            // Handle failure immediately
            if (result is Result<IEnumerable<Category>>.Failure failure)
            {
                return ResultHelpers.ToFailure<IReadOnlyList<CategoryNodeDto>>(failure.FailureMessage);
            }

            // At this point, result is a success
            var success = (Result<IEnumerable<Category>>.Success)result;

            // Additional logic: build hierarchical tree
            IReadOnlyList<CategoryNodeDto> tree = CategoryTreeBuilder.BuildTree(success.Data);

            return ResultHelpers.ToResult(tree);
        }

    }
}
