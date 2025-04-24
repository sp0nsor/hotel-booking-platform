using HotelService.Application.RequestHandlers.Commands.Hotel.Delete;
using HotelService.Application.DTOs;
using HotelService.Application.RequestHandlers.Queries.Hotel.GetById;
using HotelService.Application.RequestHandlers.Commands.Hotel.Create;
using HotelService.Application.RequestHandlers.Commands.Hotel.Update;
using HotelService.Application.RequestHandlers.Queries.Hotel.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HotelService.API.Requests.Hotels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using HotelService.API.Extensions;
using HotelService.API.Hubs;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<EntityHub<HotelDto>> _hubContext;
        private readonly IMediator _mediator;

        public HotelsController(
            IMapper mapper,
            IMediator mediator,
            IHubContext<EntityHub<HotelDto>> hubContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateHotel(
            [FromForm] CreateHotelRequest createHotelRequest,
            CancellationToken cancellationToken)
        {
            var createHotelCommand = _mapper.Map<CreateHotelCommand>(createHotelRequest);

            var result = await _mediator.Send(createHotelCommand, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels(
            [FromQuery] int PageIndex,
            [FromQuery] int PageSize,
            CancellationToken cancellationToken)
        {
            var query = new GetHotelsQuery(PageIndex, PageSize);

            var hotelsPage = await _mediator.Send(query, cancellationToken);

            return Ok(hotelsPage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHotelById(
                [FromRoute] Guid id,
                CancellationToken cancellationToken)
        {
            var query = new GetHotelByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHotel(
            [FromRoute] Guid id,
            [FromForm] UpdateHotelRequest updateHotelRequest,
            CancellationToken cancellationToken)
        {
            var updateHotelCommand = _mapper.Map<UpdateHotelCommand>(updateHotelRequest);
            updateHotelCommand.Id = id;

            var result = await _mediator.Send(updateHotelCommand, cancellationToken);

            result.WhenSuccess(async hotel =>
                await _hubContext.Clients.All
                    .SendAsync("ReceiveMessage", hotel, cancellationToken));

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteHotelCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}