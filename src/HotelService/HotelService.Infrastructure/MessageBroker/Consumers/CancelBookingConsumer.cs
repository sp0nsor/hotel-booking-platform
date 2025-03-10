using MassTransit;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public class CancelBookingConsumer 
        : IConsumer<CancelBookingEvent>
    {
        public Task Consume(ConsumeContext<CancelBookingEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
