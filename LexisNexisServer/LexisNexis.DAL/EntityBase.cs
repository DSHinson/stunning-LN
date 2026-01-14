using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL
{
    /// <summary>
    /// Provides a base class for entities with a strongly typed identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public abstract class EntityBase<TKey>
    {
        public required virtual TKey Id { get; set; }
    }

    /// <summary>
    /// Convenience base class for entities with an integer identifier.
    /// </summary>
    public abstract class EntityBase : EntityBase<int>
    {
    }
}
