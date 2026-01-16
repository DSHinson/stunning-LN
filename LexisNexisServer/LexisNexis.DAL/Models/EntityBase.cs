namespace LexisNexis.DAL.Models
{
    /// <summary>
    /// Provides a base class for entities with a strongly typed identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public abstract record EntityBase<TKey>
    {
        public required virtual TKey Id { get; set; }
    }

    /// <summary>
    /// Convenience base class for entities with an integer identifier.
    /// </summary>
    public abstract record EntityBase : EntityBase<int>
    {
    }
}
