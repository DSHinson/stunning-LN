using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Command
{
    /// <summary>
    /// Represents a command that can be executed within the application.
    /// </summary>
    public interface ICommand<TResult> : IEvent;
}
