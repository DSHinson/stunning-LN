using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Command
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Asynchronously handles the specified command and returns a result upon completion.
        /// </summary>
        /// <param name="command">
        /// The command to be processed. Cannot be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the outcome of processing the
        /// command.
        /// </returns>
        Task<TResult> HandleAsync(TCommand command);
    }
}
