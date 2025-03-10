using MassTransit;

namespace HotelService.Infrastructure.MessageBroker.Consumers
{
    public class UpdateBookingConsumer : IConsumer<UpdateBookingConsumer>
    {
        public Task Consume(ConsumeContext<UpdateBookingConsumer> context)
        {
            throw new NotImplementedException();
        }
    }
}
