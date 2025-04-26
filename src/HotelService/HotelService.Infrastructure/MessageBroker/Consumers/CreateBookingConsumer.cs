using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Create;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public sealed class CreateBookingConsumer
        : IConsumer<CreateBookingEvent>
    {
        private readonly ILogger<CreateBookingConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateBookingConsumer(
            IMediator mediator,
            IMapper mapper,
            ILogger<CreateBookingConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateBookingEvent> context)
        {
            var createBookingCommand = _mapper.Map<CreateBookingCommand>(context.Message);

            var result = await _mediator.Send(createBookingCommand, context.CancellationToken);

            if (result.IsFailure)
                _logger.LogError($"create booking error: {result.Error}");
        }
    }
}
