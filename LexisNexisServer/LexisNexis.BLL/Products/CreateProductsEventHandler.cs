using LexisNexis.Common.Cache;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class CreateProductsEventHandler : ICommandHandler<CreateProductEvent, Result<Product>>
    {
        IWriteRepository<Product, int> _repo;
        private readonly ICacheService _cacheService;

        public CreateProductsEventHandler(IWriteRepository<Product, int> repo, ICacheService cacheService)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<Result<Product>> HandleAsync(CreateProductEvent command)
        {
            if (command is not { Name: { Length: > 2 }, SKU: { Length: > 0 }, Price: > 0, Quantity: >= 0 })
            {
                return ResultHelpers.ToFailure<Product>("Invalid product data.");
            }

            var result = await _repo.AddAsync(new Product
            {
                Id = 0,
                Name = command.Name,
                CategoryId = command.CategoryId,
                Description = command.Description,
                SKU = command.SKU,
                Price = command.Price,
                Quantity = command.Quantity,
                CreatedAt = DateTime.UtcNow,
            });

            if (result is Result<Product>.Success)
            {
                _cacheService.EvictCache();
            }

            return result;

        }
    }
}
