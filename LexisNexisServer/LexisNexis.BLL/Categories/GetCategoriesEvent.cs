using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.BLL.Products
{
    [EventReplayBehaviorAttribute(EventReplayOptions.Replayable)]
    public class GetCategoriesEvent : IQuery<Result<IEnumerable<Category>>>
    {
    }
}
