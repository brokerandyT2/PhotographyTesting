using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers.EncryptionService
{
    public interface IPhotoEncryptionService
    {
        Task<byte[]> EncryptPhotoAsync(byte[] photoBytes);
        Task<byte[]> DecryptPhotoAsync(byte[] encryptedBytes);
        Task EncryptAndSavePhotoAsync(string inputFilePath, string outputFilePath);
        Task<byte[]> LoadAndDecryptPhotoAsync(string encryptedFilePath);
    }
}
