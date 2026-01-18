using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;

namespace LexisNexis.BLL.Categories
{
    [EventReplayBehaviorAttribute(EventReplayOptions.DoNotReplay)]
    public class DeleteCategoryEvent: ICommand<Result>
    {
        public required int Id { get; init; }
    }
}
