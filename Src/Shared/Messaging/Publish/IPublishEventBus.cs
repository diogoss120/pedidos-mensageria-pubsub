namespace Messaging.Publish
{
    public interface IPublishEventBus
    {
        Task<string> Publish<T>(string projectName, string topicId, T message);
    }
}
