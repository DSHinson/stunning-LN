using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Categories
{
    [EventReplayBehavior(EventReplayOptions.DoNotReplay)]
    public class DeleteCategoryEvent: ICommand<Result>
    {
        public required int Id { get; init; }
    }
}
