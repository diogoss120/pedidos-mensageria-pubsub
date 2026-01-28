namespace Messaging.Consume
{
    public class ConsumeEventBus : IConsumeEventBus
    {
        public Task<T> PublishAsync<T>(string projectId, string subscription)
        {
            throw new NotImplementedException();
        }
    }
}
