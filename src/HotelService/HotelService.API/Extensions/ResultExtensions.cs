using CSharpFunctionalExtensions;

namespace HotelService.API.Extensions
{
    public static class ResultExtensions
    {
        public static void WhenSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
                action(result.Value);
        }
    }
}
