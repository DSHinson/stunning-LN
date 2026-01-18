using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;

namespace LexisNexis.BLL.Products
{
    [EventReplayBehaviorAttribute(EventReplayOptions.DoNotReplay)]
    public sealed class DeleteProductEvent: ICommand<Result>
    {
        public int Id { get; init; }
    }

}
