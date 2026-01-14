using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Query
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Asynchronously handles the specified query and returns a result.
        /// </summary>
        /// <param name="query">
        /// The query to be processed. Cannot be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the result of handling the
        /// query.
        /// </returns>
        Task<TResult> HandleAsync(TQuery query);
    }
}
