using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locations.Core.Data.Models;

namespace Locations.Core.Business.DataAccess.Base
{
    /// <summary>
    /// Generic base interface for all service operations
    /// </summary>
    /// <typeparam name="T">The entity type for this service</typeparam>
    public interface IServiceBase<T> where T : class
    {
        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve</param>
        /// <returns>Operation result containing the entity if found</returns>
        Task<DataOperationResult<T>> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Operation result containing a list of entities</returns>
        Task<DataOperationResult<List<T>>> GetAllAsync();

        /// <summary>
        /// Saves an entity
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>Operation result containing the saved entity with updated ID</returns>
        Task<DataOperationResult<T>> SaveAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>Operation result indicating success or failure</returns>
        Task<DataOperationResult<bool>> UpdateAsync(T entity);

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
        Task<DataOperationResult<bool>> DeleteAsync(T entity);

        /// <summary>
        /// Handles raising error events
        /// </summary>
        /// <param name="e">Error event arguments</param>
        void OnErrorOccurred(DataErrorEventArgs e);
    }
}