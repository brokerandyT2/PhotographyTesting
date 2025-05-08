using System;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Interface for media services to handle photo and video capture
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Event for error notification
        /// </summary>
        event OperationErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Captures a photo using the device camera
        /// </summary>
        /// <returns>Result containing the file path where the photo is saved</returns>
        Task<OperationResult<string>> CapturePhotoAsync();

        /// <summary>
        /// Picks a photo from the device gallery
        /// </summary>
        /// <returns>Result containing the file path where the photo is saved</returns>
        Task<OperationResult<string>> PickPhotoAsync();

        /// <summary>
        /// Checks if the device supports photo capture
        /// </summary>
        /// <returns>True if the device supports photo capture, false otherwise</returns>
        Task<bool> IsCaptureSupported();

        /// <summary>
        /// Gets the app's photo storage directory
        /// </summary>
        /// <returns>The path to the directory where photos are stored</returns>
        string GetPhotoStorageDirectory();

        /// <summary>
        /// Deletes a photo from the device
        /// </summary>
        /// <param name="filePath">Path to the photo file</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<OperationResult<bool>> DeletePhotoAsync(string filePath);
    }
}