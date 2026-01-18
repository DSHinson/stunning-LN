using LexisNexis.Common.Cache;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    internal class UpdateProductEventHandler : ICommandHandler<UpdateProductEvent, Result<Product>>
    {
        IWriteRepository<Product, int> _repo;
        ICacheService _cacheService;
        public UpdateProductEventHandler(IWriteRepository<Product, int> repo, ICacheService cacheService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public async Task<Result<Product>> HandleAsync(UpdateProductEvent command)
        {
            // Pattern matching validation
            if (command is not { Name: { Length: > 2 }, Price: > 0, Quantity: >= 0 })
            {
                return ResultHelpers.ToFailure<Product>("Invalid product data");
            }

            var result = await _repo.UpdateAsync(new Product
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                SKU = command.SKU,
                Price = command.Price,
                Quantity = command.Quantity,
                CategoryId = command.CategoryId,
                UpdatedAt = DateTime.UtcNow
            });

            if (result is Result<Product>.Success)
            {
                _cacheService.EvictCache();
            }
            return result;
        }
    }
}
