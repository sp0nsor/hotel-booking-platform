using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Delete
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly IImageService imageService;
        private readonly IRepository<Core.Models.Room> roomRepository;

        public DeleteRoomCommandHandler(
            IImageService imageService,
            IRepository<Core.Models.Room> roomRepository)
        {
            this.imageService = imageService;
            this.roomRepository = roomRepository;
        }

        public async Task Handle(
            DeleteRoomCommand request,
            CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if(room is null)
                return;

            var deleteRoomImageTask = imageService.DeleteImageAsync(
                room.Image.Value,
                cancellationToken);

            await roomRepository.DeleteAsync(room, cancellationToken);
            await deleteRoomImageTask;
        }
    }
}
