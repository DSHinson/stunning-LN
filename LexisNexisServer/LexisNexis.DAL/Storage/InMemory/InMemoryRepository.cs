using LexisNexis.Common.Result;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace LexisNexis.DAL.Storage.InMemory
{
    public class InMemoryRepository<T, TKey> : IWriteRepository<T, TKey>, IReadRepository<T, TKey> where T : EntityBase<TKey> where TKey : notnull
    {
        private const int _maxWaitMilliseconds = 5000;
        private readonly ConcurrentDictionary<TKey, T> _store = new();
        private readonly IIdGenerator<TKey> _idGenerator;

        private readonly SemaphoreSlim _semaphore = new(1, 1);
        //Force a singleton pattern for in-memory repository
        private static readonly Lazy<InMemoryRepository<T, TKey>> _instance = new(() => new InMemoryRepository<T, TKey>());

        public static InMemoryRepository<T, TKey> Instance => _instance.Value;
        private InMemoryRepository() { }
        public InMemoryRepository(ConcurrentDictionary<TKey, T> store, IIdGenerator<TKey> idGenerator) 
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.AddAsync(T)"/>
        public async Task<Result<T>> AddAsync(T entity)
        {
            if (entity == null)
            {
                return ResultHelpers.ToFailure<T>("Entity cannot be null");
            }

            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure<T>("Unable to acquire write lock");
            }

            try
            {
                //Check for adding a duplicate item
                if (_store.ContainsKey(entity.Id))
                {
                    return ResultHelpers.ToFailure<T>("Duplicate Id");
                }

                entity = entity with { Id = _idGenerator.Next() };

                //Safety check we didnt some how generate a duplicate id
                if (_store.ContainsKey(entity.Id))
                {
                    return ResultHelpers.ToFailure<T>("Duplicate Id");
                }
                
                _store[entity.Id] = entity;

                return ResultHelpers.ToResult(entity);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.AddAsync(T)"/>
        public async Task<Result> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                return ResultHelpers.ToFailure("Entity cannot be null");
            }

            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure("Unable to acquire write lock");
            }

            try
            {
                if (!_store.ContainsKey(entity.Id))
                {
                    return ResultHelpers.ToFailure("Entity does not exist");
                }

                _store[entity.Id] = entity;
                return ResultHelpers.ToResult();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.RemoveAsync(T)"/>
        public async Task<Result> RemoveAsync(T entity)
        {
            if (entity == null)
            {
                return ResultHelpers.ToFailure("Entity cannot be null");
            }

            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure("Unable to acquire write lock");
            }

            try
            {
                if (!_store.TryRemove(entity.Id, out _))
                {
                    return ResultHelpers.ToFailure("Entity does not exist");
                }

                return ResultHelpers.ToResult();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.GetByIdAsync(TKey)"/>
        public async Task<Result<T>> GetByIdAsync(TKey id)
        {
            if (id == null)
            { 
               return ResultHelpers.ToFailure<T>("Id cannot be null");
            }

            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure<T>("Unable to acquire read lock");
            }

            try
            {
                if (!_store.TryGetValue(id, out T? entity))
                {
                    return ResultHelpers.ToFailure<T>("Entity does not exist");
                }

                return ResultHelpers.ToResult(entity);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.GetAllAsync(Expression{Func{T, bool}}?)"/>
        public async Task<Result<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>("Unable to acquire read lock");
            }

            try
            {
                IEnumerable<T> values = _store.Values;

                if (predicate is not null)
                {
                    values = values.Where(predicate.Compile());
                }

                return ResultHelpers.ToResult(values.ToList().AsEnumerable<T>());
            }
            finally
            {
                _semaphore.Release();
            }
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.FindAsync(Expression{Func{T, bool}})"/>
        public async Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>("Predicate cannot be null");
            }

            if (!await _semaphore.WaitAsync(_maxWaitMilliseconds))
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>("Unable to acquire read lock");
            }

            try
            {
                Func<T, bool> compiled = predicate.Compile();
                List<T> results = _store.Values.Where(compiled).ToList();
                return ResultHelpers.ToResult<IEnumerable<T>>(results);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

}
