using LexisNexis.Common.Cache;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class DeleteProductEventHandler : ICommandHandler<DeleteProductEvent, Result>
    {
        private readonly IReadRepository<Product, int> _readRepo;
        private readonly IWriteRepository<Product, int> _writeRepo;
        private readonly ICacheService _cacheService;

        public DeleteProductEventHandler(IReadRepository<Product, int> readRepo, IWriteRepository<Product, int> writeRepo, ICacheService cacheService)
        {
            _readRepo = readRepo ?? throw new ArgumentNullException(nameof(readRepo));
            _writeRepo = writeRepo ?? throw new ArgumentNullException(nameof(writeRepo));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public async Task<Result> HandleAsync(DeleteProductEvent command)
        {
            if (command is not { Id: > 0 })
            {
                return ResultHelpers.ToFailure<bool>("Invalid product id");
            }

            Result result = await _writeRepo.RemoveAsync(command.Id);

            if (result is Result.Success)
            {
                _cacheService.EvictCache();
            }
            
            return result;
        }
    }

}
