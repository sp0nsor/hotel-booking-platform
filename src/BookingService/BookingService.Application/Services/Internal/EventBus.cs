using BookingService.Application.Interfaces.Internal;
using MassTransit;

namespace BookingService.Application.Services.Internal
{
    public class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(
            T message,
            CancellationToken cancellationToken)
            where T : class
        {
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
