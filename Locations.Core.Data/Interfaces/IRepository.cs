using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locations.Core.Data.Models;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type for this repository</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Event triggered when an error occurs in the repository
        /// </summary>
        event EventHandler<DataErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve</param>
        /// <returns>Operation result containing the entity if found</returns>
        Task<DataOperationResult<TEntity>> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Operation result containing a list of entities</returns>
        Task<DataOperationResult<IList<TEntity>>> GetAllAsync();

        /// <summary>
        /// Saves an entity
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>Operation result containing the saved entity</returns>
        Task<DataOperationResult<TEntity>> SaveAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>Operation result indicating success or failure</returns>
        Task<DataOperationResult<bool>> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>Operation result indicating success or failure</returns>
        Task<DataOperationResult<bool>> DeleteAsync(int id);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>Operation result indicating success or failure</returns>
        Task<DataOperationResult<bool>> DeleteAsync(TEntity entity);
    }
}