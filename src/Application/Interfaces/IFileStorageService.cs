using System;
using Microsoft.AspNetCore.Http;
using Shared.Types;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Store file in the file storage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="directory"></param>
        /// <returns>return file path in the storage</returns>
        /// <exception cref="ErrorMessage.StoreFileError"></exception>
        Task<Result<string>> StoreFile(IFormFile file, string directory);

        /// <summary>
        /// Delete file from the file storage
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>return file path in the storage</returns>
        /// <exception cref="ErrorMessage.DeleteFileError"></exception>
        Result<string> DeleteFile(string filePath);
    }
}
