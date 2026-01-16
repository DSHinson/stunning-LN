using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Products
{
    public class DeleteProductEventHandler : ICommandHandler<DeleteProductEvent, Result>
    {
        private readonly IReadRepository<Product, int> _readRepo;
        private readonly IWriteRepository<Product, int> _writeRepo;

        public DeleteProductEventHandler(IReadRepository<Product, int> readRepo, IWriteRepository<Product, int> writeRepo)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public async Task<Result> HandleAsync(DeleteProductEvent command)
        {
            if (command is not { Id: > 0 })
            {
                return ResultHelpers.ToFailure<bool>("Invalid product id");
            }

            return await _writeRepo.RemoveAsync(command.Id);
        }
    }

}
