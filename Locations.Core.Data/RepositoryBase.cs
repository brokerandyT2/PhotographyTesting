using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;

namespace Locations.Core.Data.Queries.Base
{
    /// <summary>
    /// Base implementation for repositories
    /// </summary>
    /// <typeparam name="TEntity">The entity type for this repository</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Event triggered when an error occurs in the repository
        /// </summary>
        public event EventHandler<DataErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Raises the error event
        /// </summary>
        protected virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        public abstract Task<DataOperationResult<TEntity>> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        public abstract Task<DataOperationResult<IList<TEntity>>> GetAllAsync();

        /// <summary>
        /// Saves an entity - updated to match IRepository return type
        /// </summary>
        public abstract Task<DataOperationResult<TEntity>> SaveAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public abstract Task<DataOperationResult<bool>> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        public abstract Task<DataOperationResult<bool>> DeleteAsync(int id);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        public abstract Task<DataOperationResult<bool>> DeleteAsync(TEntity entity);

        /// <summary>
        /// Creates an error result
        /// </summary>
        protected DataOperationResult<T> CreateErrorResult<T>(ErrorSource source, string message, Exception ex)
        {
            return DataOperationResult<T>.Failure(source, message, ex);
        }
    }
}