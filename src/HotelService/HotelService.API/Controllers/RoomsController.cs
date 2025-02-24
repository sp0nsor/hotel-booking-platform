using HotelService.API.Contracts;
using HotelService.Application.RequestHandlers.Commands.Room.Create;
using HotelService.Application.RequestHandlers.Commands.Room.Delete;
using HotelService.Application.RequestHandlers.Commands.Room.Update;
using HotelService.Application.RequestHandlers.Queries.Room.Get;
using HotelService.Application.RequestHandlers.Queries.Room.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("hotels/{hotelId}/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator mediator;

        public RoomsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> AddRoomToHotel(
            [FromRoute] Guid hotelId,
            [FromForm] CreateRoomRequest roomRequest,
            CancellationToken cancellationToken)
        {
            var createRoomCommand = new CreateRoomCommand(
                hotelId,
                roomRequest.Area,
                roomRequest.Number,
                roomRequest.Capacity,
                roomRequest.MoneyAmount,
                roomRequest.Currency,
                roomRequest.Image);

            var result = await mediator.Send(createRoomCommand, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetHotelRooms(
            [FromRoute] Guid hotelId,
            [FromQuery] int PageIndex,
            [FromQuery] int PageSize, 
            CancellationToken cancellationToken)
        {
            var query = new GetRoomsQuery(hotelId, PageIndex, PageSize);

            var result = await mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomById(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetRoomByIdQuery(id, hotelId);

            var result = await mediator.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid id,
            [FromForm] UpdateRoomRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoomCommand(
                id,
                hotelId,
                request.Area,
                request.Number,
                request.Capacity,
                request.MoneyAmount,
                request.Currency,
                request.Image);

            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoomCommand(id);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
