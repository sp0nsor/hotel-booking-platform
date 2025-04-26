using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Update;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public class UpdateBookingConsumer
        : IConsumer<UpdateBookingEvent>
    {
        private readonly ILogger<UpdateBookingConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdateBookingConsumer(
            IMediator mediator,
            IMapper mapper,
            ILogger<UpdateBookingConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UpdateBookingEvent> context)
        {
            var updateBookingCommand = _mapper.Map<UpdateBookingCommand>(context.Message);

            var result = await _mediator.Send(updateBookingCommand, context.CancellationToken);

            if (result.IsFailure)
                _logger.LogError($"update booking error: {result.Error}");
        }
    }
}
