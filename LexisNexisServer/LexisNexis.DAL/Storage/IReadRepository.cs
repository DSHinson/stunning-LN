using LexisNexis.Common.Result;
using System.Linq.Expressions;

namespace LexisNexis.DAL.Storage
{
    /// <summary>
    /// Defines a read-only repository interface for retrieving entities by identifier, querying all entities, or
    /// searching with a predicate.
    /// </summary>
    /// <remarks>This interface provides methods for accessing entities without supporting modification
    /// operations. Implementations may vary in data source and retrieval strategy. Thread safety and query performance
    /// depend on the specific implementation.</remarks>
    /// <typeparam name="T">The type of entity managed by the repository. Must inherit from EntityBase<TKey>.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    public interface IReadRepository<T, TKey> where T : EntityBase<TKey>
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null.</param>
        /// <returns>The entity that matches the specified identifier, or null if no such entity exists.</returns>
        Task<Result<T>> GetByIdAsync(TKey id);

        /// <summary>
        /// Retrieves all items in the collection.
        /// </summary>
        /// <returns>An enumerable collection of items of type <typeparamref name="T"/>. The collection will be empty if no items
        /// are present.</returns>
        /// <param name="predicate">Optional predicate to filter the results.</param>
        Task<Result<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);


        /// <summary>
        /// Retrieves a collection of entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">An expression that defines the conditions each entity must meet to be included in the result. Cannot be
        /// null.</param>
        /// <returns>An enumerable collection of entities of type T that match the given predicate. If no entities match, the
        /// collection will be empty.</returns>
        Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
