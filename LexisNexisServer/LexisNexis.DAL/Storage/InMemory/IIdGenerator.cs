using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Storage.InMemory
{
    /// <summary>
    /// Defines a mechanism for generating unique identifiers of a specified type.
    /// </summary>
    /// <remarks>Implementations of this interface can be used to produce unique or sequential keys for
    /// entities, records, or other objects. The specific strategy for generating identifiers may vary depending on the
    /// implementation.</remarks>
    /// <typeparam name="TKey">The type of identifier to generate.</typeparam>
    public interface IIdGenerator<TKey>
    {
        TKey Next();
    }

}
