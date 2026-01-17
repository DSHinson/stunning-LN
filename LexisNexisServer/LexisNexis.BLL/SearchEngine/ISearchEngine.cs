using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.BLL.SearchEngine
{
    /// <summary>
    /// Defines a generic search engine for entities of type T with key type TKey.
    /// Supports weighted and fuzzy search across string properties.
    /// </summary>
    /// <typeparam name="T">The entity type, must inherit from EntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface ISearchEngine<T, TKey> where T : EntityBase<TKey>
    {
        /// <summary>
        /// Searches for entities that match the specified query string, applying weighted and fuzzy scoring.
        /// </summary>
        /// <param name="query">The search query to match against the entity's string properties.</param>
        /// <returns>A Result containing an ordered collection of matching entities, or a failure.</returns>
        Task<Result<IEnumerable<T>>> Search(string query);
    }
}
