using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.BLL.Products
{
    [EventReplayBehaviorAttribute(EventReplayOptions.Replayable)]
    public class GetProductsEvent : IQuery<Result<IEnumerable<Product>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? Category { get; set; }
        public string? Search { get; set; }
    }
}
