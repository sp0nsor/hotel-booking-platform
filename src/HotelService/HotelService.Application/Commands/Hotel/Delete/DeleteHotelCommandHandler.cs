using CSharpFunctionalExtensions;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.Commands.Hotel.DeleteHotel
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Result<Guid>>
    {
        private readonly IHotelRepository hotelRepository;

        public DeleteHotelCommandHandler(
            IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task<Result<Guid>> Handle(
            DeleteHotelCommand request,
            CancellationToken cancellationToken)
        {
            var id = await hotelRepository.Delete(request.id, cancellationToken);

            if (string.IsNullOrEmpty(id.ToString()))
                return Result.Failure<Guid>("Hotel not found");

            return Result.Success<Guid>(id);
        }
    }
}
