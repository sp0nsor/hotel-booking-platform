namespace BookingService.Application.Interfaces.Internal
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
    }
}