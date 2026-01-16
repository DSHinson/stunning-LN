using LexisNexis.BLL.Categories;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;

namespace LexisNexis.BLL.Products
{
    public class CreateCategoryEventHandler : ICommandHandler<CreateCategoryEvent, Result<Category>>
    {
        IWriteRepository<Category, int> _repo;

        public CreateCategoryEventHandler(IWriteRepository<Category,int> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Result<Category>> HandleAsync(CreateCategoryEvent command)
        {
            if (command is not { Name: { Length: > 2 } , Description: { Length: > 5 }})
            { 
                return ResultHelpers.ToFailure<Category>("Invalid Category data.");
            }

           return await _repo.AddAsync(new Category
            {
                Id = 0,
                Name = command.Name,
                Description = command.Description,
                ParentCategoryId = command.ParentCategoryId
           });
        }
    }
}
