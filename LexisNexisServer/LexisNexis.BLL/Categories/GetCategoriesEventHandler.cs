using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class GetCategoriesEventHandler : IQueryHandler<GetCategoriesEvent, Result<IEnumerable<Category>>>
    {
        IReadRepository<Category, int> _repo;

        public GetCategoriesEventHandler(IReadRepository<Category, int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Result<IEnumerable<Category>>> HandleAsync(GetCategoriesEvent query)
        {
            return await _repo.GetAllAsync();
        }
    }
}
