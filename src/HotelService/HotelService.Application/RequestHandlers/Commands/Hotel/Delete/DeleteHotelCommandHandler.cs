using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
    {
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public DeleteHotelCommandHandler(
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task Handle(
            DeleteHotelCommand request,
            CancellationToken cancellationToken)
        {
            await hotelRepository.DeleteAsync(request.id, cancellationToken);
        }
    }
}
