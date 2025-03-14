using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Update;
using MassTransit;
using MediatR;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public class UpdateBookingConsumer : IConsumer<UpdateBookingEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdateBookingConsumer(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UpdateBookingEvent> context)
        {
            var updateBookingCommand = _mapper.Map<UpdateBookingCommand>(context.Message);

            var result = await _mediator.Send(updateBookingCommand, context.CancellationToken);

            if(result.IsFailure) // elk
                Console.WriteLine(result.Error);
        }
    }
}
