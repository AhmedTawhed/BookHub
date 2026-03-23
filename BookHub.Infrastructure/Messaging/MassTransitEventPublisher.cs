using BookHub.Core.Interfaces;
using MassTransit;

namespace BookHub.Infrastructure.Messaging
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T message) where T : class
            => _publishEndpoint.Publish(message);
    }
}
