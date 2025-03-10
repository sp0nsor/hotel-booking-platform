using MassTransit;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public sealed class CreateBookingConsumer
        : IConsumer<CreateBookingEvent>
    {
        public Task Consume(ConsumeContext<CreateBookingEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
