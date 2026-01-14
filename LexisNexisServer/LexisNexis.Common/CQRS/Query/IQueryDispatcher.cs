using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Query
{
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Asynchronously dispatches the specified query to the appropriate handler and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">
        /// The type of the query to dispatch. Must implement IQuery<TResult>.
        /// </typeparam>
        /// <param name="query">
        /// The query to be dispatched. Cannot be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the result produced by handling the query.
        /// </returns>
        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query);
    }
}
