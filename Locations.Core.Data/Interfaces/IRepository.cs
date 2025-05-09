using Locations.Core.Data.Models;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Event raised when a data error occurs
        /// </summary>
        event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        Task<DataOperationResult<TEntity>> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities of type TEntity
        /// </summary>
        Task<DataOperationResult<IList<TEntity>>> GetAllAsync();

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        Task<DataOperationResult<bool>> DeleteAsync(int id);

        /// <summary>
        /// Saves an entity
        /// </summary>
        Task<DataOperationResult<bool>> SaveAsync(TEntity entity);

        /// <summary>
        /// Updates an entity
        /// </summary>
        Task<DataOperationResult<bool>> UpdateAsync(TEntity entity);

        /// <summary>
        /// Saves an entity and returns it with ID populated
        /// </summary>
        Task<DataOperationResult<TEntity>> SaveWithIdReturnAsync(TEntity entity);
    }
}