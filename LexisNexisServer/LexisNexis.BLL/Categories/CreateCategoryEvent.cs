using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.BLL.Categories
{
    [EventReplayBehaviorAttribute(EventReplayOptions.MutatesData)]
    public sealed class CreateCategoryEvent: ICommand<Result<Category>>
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public int? ParentCategoryId { get; init; }
    }
}
