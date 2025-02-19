using HotelService.Application.Commands.Hotel.CreateHotel;
using HotelService.Application.Commands.Hotel.DeleteHotel;
using HotelService.Application.Commands.Hotel.UpdateHotel;
using HotelService.Application.DTOs;
using HotelService.Application.Queries.Hotel.Get;
using HotelService.Application.Queries.Hotel.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IMediator mediator;

        public HotelController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateHotel(
            [FromForm] CreateHotelCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels(
            [FromQuery] GetHotelsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHotelById(
                [FromRoute] Guid id,
                CancellationToken cancellationToken)
        {
            var query = new GetHotelByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHotel(
            [FromBody] UpdateHotelCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteHotelCommand(id);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
                return NotFound(result.Error);

            return NoContent();
        }
    }
}