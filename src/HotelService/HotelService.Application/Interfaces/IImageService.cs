using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> WriteImageAsync(IFormFile image, CancellationToken cancellationToken);
        Task DeleteImageAsync(string imagePath, CancellationToken cancellationToken);
    }
}