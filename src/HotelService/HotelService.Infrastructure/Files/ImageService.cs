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

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private readonly static string StaticFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");

        public async Task<string> WriteImageAsync(
            IFormFile image,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(image.GetHashCode());
            var fileExtention = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(fileExtention))
                throw new Exception("Unsupported image format");

            if (image.Length > MaxFileSize)
                throw new Exception("Image size can not be more then 5 MB");

            if (!Directory.Exists(StaticFilePath))
                Directory.CreateDirectory(StaticFilePath);

            var fileName = Guid.NewGuid().ToString() + fileExtention;
            var fullPath = Path.Combine(StaticFilePath, fileName);

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream, cancellationToken);
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when creating file {ex.Message}");
            }

        }

        public async Task DeleteImageAsync(
            string imagePath,
            CancellationToken cancellationToken)
        {
            if (!File.Exists(imagePath))
                throw new Exception("Invalid file path");

            try
            {
                await Task.Run(() => File.Delete(imagePath), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when deleting file: {ex.Message}");
            }
        }
    }
}
