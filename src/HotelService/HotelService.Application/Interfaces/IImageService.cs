using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> WriteImage(IFormFile image, CancellationToken cancellationToken);
    }
}