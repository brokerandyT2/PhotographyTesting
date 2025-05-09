using System;
using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;

namespace Locations.Core.Data.Helpers
{
    /// <summary>
    /// Provides a centralized way to manage database encryption keys
    /// </summary>
    public static class EncryptionKeyProvider
    {
        /// <summary>
        /// Gets the encryption key derived from user email and unique ID
        /// </summary>
        /// <returns>Encryption key as string</returns>
        /// <exception cref="InvalidOperationException">Thrown when required settings are missing</exception>
        public static string GetEncryptionKey()
        {
            string email = NativeStorageService.GetSetting(MagicStrings.Email);
            string guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(guid))
            {
                throw new InvalidOperationException("Email and UniqueID must be set for database encryption");
            }

            return guid + email;
        }
    }
}