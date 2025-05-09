using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using EncryptedSQLite;
using Locations.Core.Data.Helpers;
using Locations.Core.Data.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Base generic repository implementation with common CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type this repository handles</typeparam>
    public class RepositoryBase<TEntity> : Database, IRepository<TEntity> where TEntity : class, new()
    {
        public RepositoryBase(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<TEntity>> GetByIdAsync(int id)
        {
            try
            {
                // Assumption: Entity has a property named "Id" or "ID"
                // This would need to be adapted based on your actual entity property names
                // Use Expression<Func<T, bool>> for FirstOrDefaultAsync
                var idProperty = typeof(TEntity).GetProperty("Id") ?? typeof(TEntity).GetProperty("ID");
                if (idProperty == null)
                {
                    throw new InvalidOperationException($"Entity {typeof(TEntity).Name} does not have an Id or ID property");
                }

                // Create a parameter expression for the lambda
                var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TEntity), "e");

                // Create the property access expression: e.Id or e.ID
                var property = System.Linq.Expressions.Expression.Property(parameter, idProperty);

                // Create the constant expression for the id value
                var constant = System.Linq.Expressions.Expression.Constant(id);

                // Create the equality expression: e.Id == id
                var equality = System.Linq.Expressions.Expression.Equal(property, constant);

                // Create the lambda expression: e => e.Id == id
                var lambda = System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

                var result = await dataB.Table<TEntity>().FirstOrDefaultAsync(lambda);

                if (result == null)
                {
                    return DataOperationResult<TEntity>.Failure(
                        ErrorSource.Database,
                        $"Entity with ID {id} not found");
                }

                return DataOperationResult<TEntity>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving entity ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TEntity>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving entity ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TEntity>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all entities of type TEntity
        /// </summary>
        public virtual async Task<DataOperationResult<IList<TEntity>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<TEntity>().ToListAsync();
                return DataOperationResult<IList<TEntity>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving entities: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<TEntity>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving entities: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<TEntity>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var itemResult = await GetByIdAsync(id);
                if (!itemResult.IsSuccess)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.Database,
                        $"Failed to find entity with ID {id} for deletion");
                }

                var result = await dataB.DeleteAsync(itemResult.Data);
                return result > 0
                    ? DataOperationResult<bool>.Success(true)
                    : DataOperationResult<bool>.Failure(ErrorSource.Database, "Entity was not deleted");
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while deleting entity ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting entity ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves an entity to the database
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> SaveAsync(TEntity entity)
        {
            try
            {
                // Check if entity implements IValidatable
                if (entity is IValidatable validatable)
                {
                    if (!validatable.Validate(out List<string> errors))
                    {
                        string validationErrors = string.Join(", ", errors);
                        string message = $"Validation failed: {validationErrors}";

                        LoggerService.LogWarning(message);
                        OnErrorOccurred(new DataErrorEventArgs(
                            ErrorSource.ModelValidation,
                            message,
                            null,
                            entity));

                        return DataOperationResult<bool>.Failure(
                            ErrorSource.ModelValidation,
                            message);
                    }
                }

                var result = await dataB.InsertOrReplaceAsync(entity);
                return result > 0
                    ? DataOperationResult<bool>.Success(true)
                    : DataOperationResult<bool>.Failure(ErrorSource.Database, "Entity was not saved");
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while saving entity: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex,
                    entity));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving entity: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    entity));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an entity in the database
        /// </summary>
        public virtual Task<DataOperationResult<bool>> UpdateAsync(TEntity entity)
        {
            // Update is the same as save in this implementation
            return SaveAsync(entity);
        }

        /// <summary>
        /// Saves an entity and returns it with ID populated
        /// </summary>
        public virtual async Task<DataOperationResult<TEntity>> SaveWithIdReturnAsync(TEntity entity)
        {
            try
            {
                // Check if entity implements IValidatable
                if (entity is IValidatable validatable)
                {
                    if (!validatable.Validate(out List<string> errors))
                    {
                        string validationErrors = string.Join(", ", errors);
                        string message = $"Validation failed: {validationErrors}";

                        LoggerService.LogWarning(message);
                        OnErrorOccurred(new DataErrorEventArgs(
                            ErrorSource.ModelValidation,
                            message,
                            null,
                            entity));

                        return DataOperationResult<TEntity>.Failure(
                            ErrorSource.ModelValidation,
                            message);
                    }
                }

                // Get a synchronous connection for this operation
                SQLiteConnection conn = GetSyncEncryptedConnection();
                conn.Insert(entity);

                return DataOperationResult<TEntity>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while saving entity with ID return: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex,
                    entity));
                return DataOperationResult<TEntity>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving entity with ID return: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    entity));
                return DataOperationResult<TEntity>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        // Removed GetIdPredicate method as we're creating expressions inline
    }
}