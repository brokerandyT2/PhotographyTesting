using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Locations.Core.Shared.Helpers;
//using Location.Core.Helpers.LoggingService;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Service for handling media operations (photos, videos)
    /// </summary>
    public class MediaService : IMediaService
    {
        //private readonly ILoggerService? _loggerService;

        /// <summary>
        /// Event for error notification
        /// </summary>
        public event OperationErrorEventHandler? ErrorOccurred;

        /// <summary>
        /// Constructor with optional logger
        /// </summary>
       /* public MediaService(ILoggerService? loggerService = null)
        {
            _loggerService = loggerService;
        } */

        /// <summary>
        /// Captures a photo using the device camera
        /// </summary>
        public async Task<OperationResult<string>> CapturePhotoAsync()
        {
            try
            {
                if (!await IsCaptureSupported())
                {
                    string message = "This device does not support photo capture";
                    //_loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Permission,
                        message));
                    return OperationResult<string>.Failure(
                        OperationErrorSource.Permission,
                        message);
                }

                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo == null)
                {
                    string message = "Photo capture was canceled or failed";
                   // _loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        message));
                    return OperationResult<string>.Failure(
                        OperationErrorSource.Unknown,
                        message);
                }

                // Save the file into local storage
                string localFilePath = Path.Combine(GetPhotoStorageDirectory(), photo.FileName);

                using (Stream sourceStream = await photo.OpenReadAsync())
                using (FileStream localFileStream = File.OpenWrite(localFilePath))
                {
                    await sourceStream.CopyToAsync(localFileStream);
                }

              //  _loggerService?.LogInformation($"Photo saved to {localFilePath}");
                return OperationResult<string>.Success(localFilePath);
            }
            catch (PermissionException ex)
            {
                string message = $"Permission error: {ex.Message}";
              //  _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<string>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (Exception ex)
            {
                string message = $"Error capturing photo: {ex.Message}";
              //  _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<string>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Picks a photo from the device gallery
        /// </summary>
        public async Task<OperationResult<string>> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo == null)
                {
                    string message = "Photo selection was canceled";
              //      _loggerService?.LogInformation(message);
                    return OperationResult<string>.Failure(
                        OperationErrorSource.Unknown,
                        message);
                }

                // Save the file into local storage
                string localFilePath = Path.Combine(GetPhotoStorageDirectory(), photo.FileName);

                using (Stream sourceStream = await photo.OpenReadAsync())
                using (FileStream localFileStream = File.OpenWrite(localFilePath))
                {
                    await sourceStream.CopyToAsync(localFileStream);
                }

              //  _loggerService?.LogInformation($"Photo saved to {localFilePath}");
                return OperationResult<string>.Success(localFilePath);
            }
            catch (PermissionException ex)
            {
                string message = $"Permission error: {ex.Message}";
              //  _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<string>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (Exception ex)
            {
                string message = $"Error picking photo: {ex.Message}";
             //   _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<string>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Checks if the device supports photo capture
        /// </summary>
        public async Task<bool> IsCaptureSupported()
        {
            try
            {
                return MediaPicker.Default.IsCaptureSupported;
            }
            catch (Exception ex)
            {
                string message = $"Error checking capture support: {ex.Message}";
            //    _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return false;
            }
        }

        /// <summary>
        /// Gets the app's photo storage directory
        /// </summary>
        public string GetPhotoStorageDirectory()
        {
            string directory = Path.Combine(FileSystem.AppDataDirectory, "Photos");

            // Ensure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        /// <summary>
        /// Deletes a photo from the device
        /// </summary>
        public async Task<OperationResult<bool>> DeletePhotoAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    string message = "File path cannot be null or empty";
              //      _loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.ModelValidation,
                        message));
                    return OperationResult<bool>.Failure(
                        OperationErrorSource.ModelValidation,
                        message);
                }

                if (!File.Exists(filePath))
                {
                    string message = $"File does not exist: {filePath}";
              //      _loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        message));
                    return OperationResult<bool>.Failure(
                        OperationErrorSource.Unknown,
                        message);
                }

                File.Delete(filePath);

           //     _loggerService?.LogInformation($"Deleted photo: {filePath}");
                return OperationResult<bool>.Success(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                string message = $"Permission error deleting file: {ex.Message}";
            //    _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting photo: {ex.Message}";
            //    _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Raises the ErrorOccurred event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}