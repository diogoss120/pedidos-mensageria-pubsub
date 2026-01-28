namespace Messaging.Publish
{
    public class PublishEventBus : IPublishEventBus
    {
        public void Publish<T>(string projectName, string topicId, T message)
        {
            throw new NotImplementedException();
        }
    }
}
