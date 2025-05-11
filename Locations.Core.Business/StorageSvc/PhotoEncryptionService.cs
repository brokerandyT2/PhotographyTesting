using Locations.Core.Business.StorageSvc;
using Locations.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers.EncryptionService
{
    public class PhotoEncryptionService : IPhotoEncryptionService
    {
       // private static readonly string EncryptionKey = "your-static-key-if-needed"; // Optional

        private async Task<string> GetSaltAsync()
        {
            var email =  NativeStorageService.GetSetting(MagicStrings.Email) ?? throw new InvalidOperationException("Email not found");
            var guid = NativeStorageService.GetSetting(MagicStrings.UniqueID) ?? throw new InvalidOperationException("UniqueId not found");
            return guid + email;
        }

        private async Task<byte[]> GetAesKeyAsync()
        {
            using var sha256 = SHA256.Create();
            var salt = await GetSaltAsync();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(salt));
        }

        public async Task<byte[]> EncryptPhotoAsync(byte[] photoBytes)
        {
            var key = await GetAesKeyAsync();
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                cs.Write(photoBytes, 0, photoBytes.Length);

            return ms.ToArray();
        }

        public async Task<byte[]> DecryptPhotoAsync(byte[] encryptedBytes)
        {
            var key = await GetAesKeyAsync();

            using var aes = Aes.Create();
            aes.Key = key;

            var iv = new byte[16];
            Array.Copy(encryptedBytes, 0, iv, 0, 16);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(encryptedBytes, 16, encryptedBytes.Length - 16);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var resultStream = new MemoryStream();
            cs.CopyTo(resultStream);

            return resultStream.ToArray();
        }

        public async Task EncryptAndSavePhotoAsync(string inputFilePath, string outputFilePath)
        {
            var photoBytes = await File.ReadAllBytesAsync(inputFilePath);
            var encrypted = await EncryptPhotoAsync(photoBytes);
            await File.WriteAllBytesAsync(outputFilePath, encrypted);
        }

        public async Task<byte[]> LoadAndDecryptPhotoAsync(string encryptedFilePath)
        {
            var encrypted = await File.ReadAllBytesAsync(encryptedFilePath);
            return await DecryptPhotoAsync(encrypted);
        }
    }

}
