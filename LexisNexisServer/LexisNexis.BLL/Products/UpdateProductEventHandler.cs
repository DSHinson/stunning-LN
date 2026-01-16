using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    internal class UpdateProductEventHandler : ICommandHandler<UpdateProductEvent, Result<Product>>
    {
        IWriteRepository<Product, int> _repo;
        public UpdateProductEventHandler(IWriteRepository<Product, int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public async Task<Result<Product>> HandleAsync(UpdateProductEvent command)
        {
            // Pattern matching validation
            if (command is not { Name: { Length: > 2 }, Price: > 0, Quantity: >= 0 })
            {
                return ResultHelpers.ToFailure<Product>("Invalid product data");
            }

            return await _repo.UpdateAsync(new Product
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
        }
    }
}
