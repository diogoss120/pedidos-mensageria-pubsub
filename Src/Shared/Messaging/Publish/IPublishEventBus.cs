namespace Messaging.Publish
{
    public interface IPublishEventBus
    {
        void Publish<T>(string projectName, string topicId, T message);
    }
}
