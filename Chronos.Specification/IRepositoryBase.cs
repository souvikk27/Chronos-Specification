using System.Linq.Expressions;

namespace Chronos.Specification
{
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Lists all entities.
        /// </summary>
        /// <returns>An IEnumerable of all entities.</returns>
        IEnumerable<T> ListAll();

        /// <summary>
        /// Lists all entities asynchronously.
        /// </summary>
        /// <returns>A Task representing an IEnumerable of all entities.</returns>
        Task<IEnumerable<T>> ListAllAsync();

        /// <summary>
        /// Lists entities based on a specification.
        /// </summary>
        /// <param name="specification">The specification to filter the entities.</param>
        /// <returns>An IReadOnlyList of entities that match the specification.</returns>
        IReadOnlyList<T> List(ISpecification<T> specification);

        /// <summary>
        /// Lists entities based on a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification to filter the entities.</param>
        /// <returns>A Task representing an IReadOnlyList of entities that match the specification.</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);

        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        T? GetById(object id);

        /// <summary>
        /// Gets an entity by its id asynchronously.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>A Task representing the entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(object id);

        /// <summary>
        /// Finds entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter the entities.</param>
        /// <returns>An IEnumerable of entities that match the predicate.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Finds entities based on a predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter the entities.</param>
        /// <returns>A Task representing an IEnumerable of entities that match the predicate.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        T Add(T entity);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A Task representing the added entity.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A Task representing the updated entity.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Checks if any entities exist that match the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter the entities.</param>
        /// <returns>True if any entities exist that match the predicate; otherwise, false.</returns>
        bool CheckExists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks if any entities exist that match the predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter the entities.</param>
        /// <returns>A Task representing a boolean value: true if any entities exist that match the predicate; otherwise, false.</returns>
        Task<bool> CheckExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A Task representing the deletion operation.</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <returns>A Task representing the save operation.</returns>
        Task SaveChangesAsync();
    }
}