using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Cancel;
using MassTransit;
using MediatR;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public class CancelBookingConsumer 
        : IConsumer<CancelBookingEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CancelBookingConsumer(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CancelBookingEvent> context)
        {
            var cancelBookingCommand = _mapper.Map<CancelBookingCommand>(context.Message);

            var result = await _mediator.Send(cancelBookingCommand, context.CancellationToken);

            if(result.IsFailure) // elk
                Console.WriteLine(result.Error);
        }
    }
}
