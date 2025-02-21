using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Delete
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly IRepository<Core.Models.Room> roomRepository;

        public DeleteRoomCommandHandler(
            IRepository<Core.Models.Room> roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public async Task Handle(
            DeleteRoomCommand request,
            CancellationToken cancellationToken)
        {
            await roomRepository.DeleteAsync(request.id, cancellationToken);
        }
    }
}
