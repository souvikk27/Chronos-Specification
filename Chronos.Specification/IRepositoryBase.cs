using System.Linq.Expressions;

namespace Chronos.Specification
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> ListAll();

        Task<IEnumerable<T>> ListAllAsync();

        IReadOnlyList<T> List(ISpecification<T> specification);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);

        T? GetById(object id);

        Task<T?> GetByIdAsync(object id);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        T Update(T entity);

        Task<T> UpdateAsync(T entity);

        void Delete(T entity);

        Task DeleteAsync(T entity);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}