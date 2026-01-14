using LexisNexis.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL
{
    /// <summary>
    /// Defines methods for adding, updating, and removing entities in a write-oriented repository.
    /// </summary>
    /// <remarks>This interface is typically implemented by classes that provide data persistence for
    /// entities. It focuses on write operations and does not include querying or retrieval methods. Implementations may
    /// enforce additional constraints or behaviors, such as transactional support or validation.</remarks>
    /// <typeparam name="T">The type of entity managed by the repository. Must inherit from EntityBase<TKey>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    public interface IWriteRepository<T, TKey> where T : EntityBase<TKey>
    {
        /// <summary>
        /// Adds the specified entity to the collection.
        /// </summary>
        /// <param name="entity">The entity to add to the collection. Cannot be null.</param>
        Task<Result> AddAsync(T entity);

        /// <summary>
        /// Updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update. Cannot be null. The entity must already exist in the data store.</param>
        Task<Result> UpdateAsync(T entity);

        /// <summary>
        /// Removes the specified entity from the collection.
        /// </summary>
        /// <param name="entity">The entity to remove from the collection. Cannot be null.</param>
        Task<Result> RemoveAsync(T entity);
    }
}
