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
            var room = await roomRepository.GetByIdAsync(request.Id);

            if(room is null)
                return;

            await roomRepository.DeleteAsync(room, cancellationToken);
        }
    }
}
