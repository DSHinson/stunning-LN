using LexisNexis.API;
using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
