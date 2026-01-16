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
    public class GetProductByIdEvent:IQuery<Result<Product>>
    {
        public required int Id { get; set; }
    }
}
