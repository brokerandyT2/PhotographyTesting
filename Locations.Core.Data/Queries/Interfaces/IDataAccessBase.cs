using System.Collections.Generic;
using System.Threading.Tasks;
using Locations.Core.Data.Models;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Interface for data access operations
    /// </summary>
    public interface IDataAccessBase
    {
        /// <summary>
        /// Event raised when a data error occurs
        /// </summary>
        event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        Task<DataOperationResult<T>> GetItemAsync<T>(int id) where T : new();

        /// <summary>
        /// Gets all items of type T
        /// </summary>
        Task<DataOperationResult<IList<T>>> GetItemsAsync<T>() where T : new();

        /// <summary>
        /// Deletes an item by ID
        /// </summary>
        Task<DataOperationResult<bool>> DeleteItemAsync<T>(int id) where T : new();

        /// <summary>
        /// Gets an item by a string identifier
        /// </summary>
        Task<DataOperationResult<T>> GetItemByStringAsync<T>(string name) where T : new();

        /// <summary>
        /// Gets a value by a string identifier
        /// </summary>
        Task<DataOperationResult<string>> GetValueByStringAsync<T>(string name) where T : new();

        /// <summary>
        /// Saves an item
        /// </summary>
        Task<DataOperationResult<bool>> SaveItemAsync<T>(T item) where T : new();

        /// <summary>
        /// Updates an item
        /// </summary>
        Task<DataOperationResult<bool>> UpdateAsync<T>(T item) where T : new();

        /// <summary>
        /// Saves an item and returns it with ID populated
        /// </summary>
        Task<DataOperationResult<T>> SaveWithIDReturnAsync<T>(T item) where T : new();
    }
}