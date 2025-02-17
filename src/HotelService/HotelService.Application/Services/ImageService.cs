using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.Services
{
    public class ImageService : IImageService
    {
        private static readonly string[] AllowedExtantions =
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private readonly static string StaticFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles\\Images");

        public async Task<Result<string>> WriteImage(
            IFormFile image,
            CancellationToken cancellationToken)
        {
            var fileExtention = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AllowedExtantions.Contains(fileExtention))
                return Result.Failure<string>("Unsupported image format");

            if (image.Length > MaxFileSize)
                return Result.Failure<string>("Image size can not be more then 5 MB");

            var fileName = Guid.NewGuid().ToString() + fileExtention;
            var fullPath = Path.Combine(StaticFilePath, fileName);

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream, cancellationToken);
                }

                return Result.Success(fullPath);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error when creating file: {ex.Message}");
            }

        }
    }
}
