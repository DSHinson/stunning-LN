using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LexisNexis.DAL.Storage.EF
{
    ///<inheritdoc cref="IReadRepository{T, TKey}"/>
    public class EfReadRepository<T, TKey> : IReadRepository<T, TKey> where T : EntityBase<TKey>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfReadRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.GetByIdAsync(TKey)"/>
        public async Task<Result<T>> GetByIdAsync(TKey id)
        {
            if (id == null)
            {
                return ResultHelpers.ToFailure<T>("Id cannot be null.");
            }

            try
            {
                var entity = await _dbSet.FindAsync(id);

                if (entity == null)
                {
                    return ResultHelpers.ToFailure<T>($"Entity with id '{id}' not found.");
                }

                return ResultHelpers.ToResult(entity);
            }
            catch (Exception ex)
            {
                return ResultHelpers.ToFailure<T>($"Error retrieving entity with id '{id}': {ex.Message}");
            }
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.GetAllAsync(Expression{Func{T, bool}}?)"/>
        public async Task<Result<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                var entities = await query.ToListAsync();

                return ResultHelpers.ToResult<IEnumerable<T>>(entities);
            }
            catch (Exception ex)
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>($"Error retrieving entities: {ex.Message}");
            }
        }

        ///<inheritdoc cref="IReadRepository{T, TKey}.FindAsync(Expression{Func{T, bool}})"/>
        public async Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>("Predicate cannot be null.");
            }

            try
            {
                var entities = await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

                return ResultHelpers.ToResult<IEnumerable<T>>(entities);
            }
            catch (Exception ex)
            {
                return ResultHelpers.ToFailure<IEnumerable<T>>($"Error finding entities: {ex.Message}");
            }
        }
    }
}
