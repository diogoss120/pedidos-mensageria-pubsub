using Google.Cloud.PubSub.V1;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Messaging.Publish
{
    public class PublishEventBus : IPublishEventBus
    {
        private readonly ConcurrentDictionary<string, PublisherClient> _publisher = new();

        public async Task Publish<T>(string projectId, string topicId, T message)
        {
            var key = $"{projectId}:{topicId}";

            var publisher = _publisher.GetOrAdd(
                key,
                _ => CreatePublisher(projectId, topicId).GetAwaiter().GetResult()
            );

            await publisher.PublishAsync(JsonSerializer.Serialize(message));
        }

        private static async Task<PublisherClient> CreatePublisher(
            string projectId,
            string topicId)
        {
            var topicName = TopicName.FromProjectTopic(projectId, topicId);
            return await PublisherClient.CreateAsync(topicName);
        }
    }
}
