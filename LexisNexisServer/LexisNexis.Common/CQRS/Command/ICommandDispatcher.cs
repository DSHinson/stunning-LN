using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Command
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Asynchronously dispatches the specified command and returns the result produced by its handler.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command to dispatch. Must implement the <see cref="ICommand"/> interface.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result returned by the command handler.
        /// </typeparam>
        /// <param name="command">
        /// The command instance to dispatch. Cannot be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous dispatch operation. The task result contains the value returned by
        /// the command handler.
        /// </returns>
        Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command);

    }
}
