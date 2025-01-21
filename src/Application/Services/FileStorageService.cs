using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Types;

namespace Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        public FileStorageService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"];
            if (string.IsNullOrEmpty(_rootPath))
            {
                throw new ArgumentException("File storage root path is not configured.");
            }
            else if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        public Result<string> DeleteFile(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(_rootPath, filePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return Result<string>.Done(filePath);
            }
            catch (Exception)
            {
                return Result<string>.Fail(ErrorMessage.DeleteFileError);
            }
        }

        public async Task<Result<string>> StoreFile(IFormFile file, string directory)
        {
            string fullPath = "";
            try
            {
                string filename = GenerateUniqueFileName(file);

                fullPath = Path.Combine(_rootPath, directory);

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                string exactPath = Path.Combine(fullPath, filename);

                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string res = Path.Combine(directory, filename).Replace("\\", "/");

                return Result<string>.Done(res);
            }
            catch (Exception)
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return Result<string>.Fail(ErrorMessage.StoreFileError);
            }
        }
        private static string GenerateUniqueFileName(IFormFile file)
        {
            return $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }
    }
}