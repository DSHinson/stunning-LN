using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LexisNexis.DAL.Storage.EF
{
    ///<inheritdoc cref="IWriteRepository{T, TKey}"/>
    public class EfWriteRepository<T, TKey> : IWriteRepository<T, TKey> where T : EntityBase<TKey>
        {
            private readonly ApplicationDbContext _context;
            private readonly DbSet<T> _dbSet;

            public EfWriteRepository(ApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _dbSet = _context.Set<T>();
            }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.AddAsync(T)"/>
        public async Task<Result<T>> AddAsync(T entity)
            {
                if (entity == null)
                {
                    return ResultHelpers.ToFailure<T>("Entity cannot be null.");
                }

                try
                {
                    await _dbSet.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return ResultHelpers.ToResult(entity);
                }
                catch (DbUpdateException ex)
                {
                    return ResultHelpers.ToFailure<T>($"Database error while adding entity: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    return ResultHelpers.ToFailure<T>($"Error adding entity: {ex.Message}");
                }
            }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.UpdateAsync(T)"/>
        public async Task<Result<T>> UpdateAsync(T entity)
            {
                if (entity == null)
                {
                    return ResultHelpers.ToFailure<T>("Entity cannot be null.");
                }

                try
                {
                    // Check if entity exists
                    var exists = await _dbSet.FindAsync(entity.Id);
                    if (exists == null)
                    {
                        return ResultHelpers.ToFailure<T>($"Entity with id '{entity.Id}' not found.");
                    }

                    // Detach the existing tracked entity to avoid conflicts
                    _context.Entry(exists).State = EntityState.Detached;

                    // Update the entity
                    _dbSet.Update(entity);
                    await _context.SaveChangesAsync();

                    return ResultHelpers.ToResult(entity);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return ResultHelpers.ToFailure<T>($"Concurrency error while updating entity: {ex.Message}");
                }
                catch (DbUpdateException ex)
                {
                    return ResultHelpers.ToFailure<T>($"Database error while updating entity: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    return ResultHelpers.ToFailure<T>($"Error updating entity: {ex.Message}");
                }
            }

        ///<inheritdoc cref="IWriteRepository{T, TKey}.RemoveAsync(TKey)"/>
        public async Task<Result> RemoveAsync(TKey entityId)
            {
                if (entityId == null)
                {
                    return ResultHelpers.ToFailure("Entity Id cannot be null.");
                }

                try
                {
                    // Check if entity exists
                    var exists = await _dbSet.FindAsync(entityId);
                    if (exists == null)
                    {
                        return ResultHelpers.ToFailure($"Entity with id '{entityId}' not found.");
                    }

                    _dbSet.Remove(exists);
                    await _context.SaveChangesAsync();

                    return ResultHelpers.ToResult();
                }
                catch (DbUpdateException ex)
                {
                    return ResultHelpers.ToFailure($"Database error while removing entity: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    return ResultHelpers.ToFailure($"Error removing entity: {ex.Message}");
                }
            }
        }
    }

