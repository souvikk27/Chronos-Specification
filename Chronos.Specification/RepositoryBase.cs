using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chronos.Specification
{
    public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity> where TEntity : class where TContext : DbContext
    {
        protected readonly TContext Context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(IRepositoryOptions<TContext> options)
        {
            Context = options.Context;
            _dbSet = Context.Set<TEntity>();
        }

        public virtual Expression<Func<TContext, DbSet<TEntity>>> DataSet() => null!;

        public virtual Expression<Func<TEntity, object>> Key() => null!;

        public IEnumerable<TEntity> ListAll()
        {
            var entity = DataSet().Compile()(Context);
            return entity.ToList();
        }

        public async Task<IEnumerable<TEntity>> ListAllAsync()
        {
            var entity = DataSet().Compile()(Context);
            return await entity.ToListAsync();
        }

        public IReadOnlyList<TEntity> List(ISpecification<TEntity> specification)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            query = ApplySpecificationList(query, specification);
            return query.ToList();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            query = ApplySpecificationList(query, specification);
            return await query.ToListAsync();
        }

        public TEntity? GetById(object id)
        {
            var keyExpression = Key() ??
                                throw new InvalidOperationException(
                                    "Key expression is not defined for this repository.");
            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            var keyPropertyType = GetKeyPropertyType(typeof(TEntity));
            var idConstant = Expression.Constant(Convert.ChangeType(id, keyPropertyType));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Invoke(keyExpression, entityParameter), idConstant),
                entityParameter
            );
            return Context.Set<TEntity>().SingleOrDefault(lambda);
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            var keyExpression = Key() ??
                                throw new InvalidOperationException(
                                    "Key expression is not defined for this repository.");
            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            var keyPropertyType = GetKeyPropertyType(typeof(TEntity));
            var idConstant = Expression.Constant(Convert.ChangeType(id, keyPropertyType));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Invoke(keyExpression, entityParameter), idConstant),
                entityParameter
            );
            return await Context.Set<TEntity>().SingleOrDefaultAsync(lambda);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public TEntity Add(TEntity entity)
        {
            var entityEntry = Context.Set<TEntity>().Add(entity);
            return entityEntry.Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entityEntry = await Context.Set<TEntity>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Task.Yield();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => Context.Set<TEntity>().Remove(entity));
        }

        public void SaveChanges()
        {
            Context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        private Type GetKeyPropertyType(Type entityType)
        {
            var keyProperty = Context.Model.FindEntityType(entityType)?.FindPrimaryKey()?.Properties.FirstOrDefault();
            if (keyProperty != null)
            {
                return keyProperty.ClrType;
            }
            throw new InvalidOperationException($"Key property not found for type {entityType.Name}");
        }

        private IQueryable<TEntity> ApplySpecificationList(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            if (specification == null)
                return query;
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.OrderingExpressions.Aggregate(query, (current, orderingExpression) =>
                orderingExpression.Direction == OrderingDirection.Ascending
                    ? current.OrderBy(orderingExpression.OrderingKeySelector!)
                    : current.OrderByDescending(orderingExpression.OrderingKeySelector!));

            if (specification.Take.HasValue)
            {
                query = query.Take(specification.Take.Value);
            }
            if (specification.Skip.HasValue)
            {
                query = query.Skip(specification.Skip.Value);
            }
            return query;
        }
    }
}