using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Query;
using LexisNexis.Common.DTO;
using LexisNexis.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Categories
{
    [EventReplayBehaviorAttribute(EventReplayOptions.Replayable)]
    public class GetCategoriesTreeEvent:IQuery<Result<IReadOnlyList<CategoryNodeDto>>>
    {
    }
}
