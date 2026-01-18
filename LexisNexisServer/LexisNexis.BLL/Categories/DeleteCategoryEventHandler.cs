using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Categories
{
    public class DeleteCategoryEventHandler : ICommandHandler<DeleteCategoryEvent, Result>
    {
        IWriteRepository<Category, int> _writeRepo;
        IReadRepository<Category, int> _readRepo;

        public DeleteCategoryEventHandler(IWriteRepository<Category, int> writeRepo, IReadRepository<Category, int> readRepo)
        {
            _writeRepo = writeRepo ?? throw new ArgumentNullException(nameof(writeRepo));
            _readRepo = readRepo ?? throw new ArgumentNullException(nameof(readRepo));
        }

        public async Task<Result> HandleAsync(DeleteCategoryEvent command)
        {
            if (command is not { Id: > 0 })
            {
                return ResultHelpers.ToFailure<Category>("Invalid Category data.");
            }

            var parentResult = await _readRepo.FindAsync(c => c.ParentCategoryId == command.Id);
            if (parentResult is Result<IEnumerable<Category>>.Success children && children.Data.Any())
            {
                return ResultHelpers.ToFailure<Category>("Can not delete a parent category");
            }
            else if (parentResult is Result<IEnumerable<Category>>.Failure)
            {
                return ResultHelpers.ToFailure<Category>("Can not delete, unable to check if a parent category");
            }

            return await _writeRepo.RemoveAsync(command.Id);
        }
    }
}
