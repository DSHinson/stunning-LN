using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.CQRS.Query
{
    /// <summary>
    /// Represents a query operation that returns a result of the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query operation.</typeparam>
    public interface IQuery<TResult>;
}
