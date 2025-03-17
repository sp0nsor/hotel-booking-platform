using HotelService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelService.Infrastructure.Files
{
    public class ImageService : IImageService
    {
        private static readonly string[] _allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        private readonly static long _maxFileSize = 5 * 1024 * 1024; // 5 MB

        private readonly static string _staticFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");

        public async Task<string> WriteImageAsync(
            IFormFile image,
            CancellationToken cancellationToken)
        {
            var fileExtention = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(fileExtention))
                throw new Exception("Unsupported image format");

            if (image.Length > _maxFileSize)
                throw new Exception("Image size can not be more then 5 MB");

            if (!Directory.Exists(_staticFilePath))
                Directory.CreateDirectory(_staticFilePath);

            var fileName = Guid.NewGuid().ToString() + fileExtention;
            var fullPath = Path.Combine(_staticFilePath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }

            return fullPath;
        }

        public async Task DeleteImageAsync(
            string imagePath,
            CancellationToken cancellationToken)
        {
            if (!File.Exists(imagePath))
                throw new Exception("Invalid file path");

            await Task.Run(() => File.Delete(imagePath), cancellationToken);
        }
    }
}
