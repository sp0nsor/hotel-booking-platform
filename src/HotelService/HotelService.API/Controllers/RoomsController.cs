using AutoMapper;
using HotelService.API.Extensions;
using HotelService.API.Hubs;
using HotelService.API.Requests.Rooms;
using HotelService.Application.DTOs;
using HotelService.Application.RequestHandlers.Commands.Room.Create;
using HotelService.Application.RequestHandlers.Commands.Room.Delete;
using HotelService.Application.RequestHandlers.Commands.Room.Update;
using HotelService.Application.RequestHandlers.Queries.Room.Get;
using HotelService.Application.RequestHandlers.Queries.Room.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("api/hotels/{hotelId}/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IHubContext<EntityHub<RoomDto>> _hubContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomsController(
            IMediator mediator,
            IMapper mapper,
            IHubContext<EntityHub<RoomDto>> hubContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddRoomToHotel(
            [FromRoute] Guid hotelId,
            [FromForm] CreateRoomRequest createRoomRequest,
            CancellationToken cancellationToken)
        {
            var createRoomCommand = _mapper.Map<CreateRoomCommand>(createRoomRequest);
            createRoomCommand.HotelId = hotelId;

            var result = await _mediator.Send(createRoomCommand, cancellationToken);

            return result.IsSuccess 
                ? Ok()
                : BadRequest(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetHotelRooms(
            [FromRoute] Guid hotelId,
            [FromQuery] int PageIndex,
            [FromQuery] int PageSize, 
            CancellationToken cancellationToken)
        {
            var query = new GetRoomsQuery(hotelId, PageIndex, PageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomById(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetRoomByIdQuery(id, hotelId);

            var result = await _mediator.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid id,
            [FromForm] UpdateRoomRequest updateRoomRequest,
            CancellationToken cancellationToken)
        {
            var updateRoomCommand = _mapper.Map<UpdateRoomCommand>(updateRoomRequest);
            updateRoomCommand.HotelId = hotelId;
            updateRoomCommand.Id = id;

            var result = await _mediator.Send(updateRoomCommand, cancellationToken);

            result.WhenSuccess(async room =>
                await _hubContext.Clients.All
                    .SendAsync("ReceiveMessage", room, cancellationToken));

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(
            [FromRoute] Guid id,
            [FromRoute] Guid hotelId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoomCommand(id, hotelId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}
