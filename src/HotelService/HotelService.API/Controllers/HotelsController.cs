using HotelService.Application.RequestHandlers.Commands.Hotel.Delete;
using HotelService.Application.DTOs;
using HotelService.Application.RequestHandlers.Queries.Hotel.GetById;
using HotelService.Application.RequestHandlers.Commands.Hotel.Create;
using HotelService.Application.RequestHandlers.Commands.Hotel.Update;
using HotelService.Application.RequestHandlers.Queries.Hotel.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator mediator;

        public HotelsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateHotel(
            [FromForm] CreateHotelCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels(
            [FromQuery] GetHotelsQuery query,
            CancellationToken cancellationToken)
        {
            var hotelsPage = await mediator.Send(query, cancellationToken);

            return Ok(hotelsPage);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHotelById(
                [FromRoute] Guid id,
                CancellationToken cancellationToken)
        {
            var query = new GetHotelByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Value);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHotel(
            [FromForm] UpdateHotelCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteHotelCommand(id);
            await mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}