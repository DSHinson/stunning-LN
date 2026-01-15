using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Retrieves an existing cache container associated with the specified key, or creates a new one if none
        /// exists.
        /// </summary>
        /// <typeparam name="T">The type of objects stored in the cache container.</typeparam>
        /// <param name="key">The unique identifier used to locate or create the cache container.</param>
        /// <returns>A cache container of type <typeparamref name="T"/> associated with the specified key. If no container exists
        /// for the key, a new one is created and returned.</returns>
        public CacheContainer<T> GetOrCreateCacheContainer<T>(Guid key);
        
        /// <summary>
        /// Stores the specified value in the cache under the given key for the specified duration.
        /// </summary>
        /// <remarks>If a value already exists for the specified key, it will be overwritten. The cached
        /// value will be automatically removed after the specified duration has elapsed.</remarks>
        /// <typeparam name="T">The type of the value to store in the cache.</typeparam>
        /// <param name="key">The unique identifier used to reference the cached value.</param>
        /// <param name="value">The value to store in the cache. Can be of any type.</param>
        /// <param name="cacheDuration">The length of time for which the value should remain in the cache before expiring.</param>
        public void Set<T>(Guid key, T value, TimeSpan cacheDuration);

        /// <summary>
        /// Removes the entry associated with the specified key from the collection.
        /// </summary>
        /// <param name="key">The unique identifier of the entry to remove from the collection.</param>
        public void Remove(Guid key);

        /// <summary>
        /// Creates a unique key by combining the specified key parts into a single GUID.
        /// </summary>
        /// <remarks>The method generates a deterministic GUID based on the values and order of the key
        /// parts. Use this method when you need a consistent key for the same set of input values.</remarks>
        /// <param name="keyParts">An array of objects representing the components to include in the key. Each part contributes to the
        /// uniqueness of the resulting GUID. Cannot contain null elements.</param>
        /// <returns>A GUID that uniquely represents the combination of the provided key parts.</returns>
        public Guid CreateKey(params object[] keyParts);
    }
}
