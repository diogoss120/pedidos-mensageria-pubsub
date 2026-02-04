namespace Messaging.Publish
{
    public interface IPublishEventBus
    {
        Task Publish<T>(string projectName, string topicId, T message);
    }
}
