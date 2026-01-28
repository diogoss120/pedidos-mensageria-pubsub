namespace Messaging.Consume
{
    public interface IConsumeEventBus
    {
        Task<T> PublishAsync<T>(string projectId, string subscription);
    }
}
