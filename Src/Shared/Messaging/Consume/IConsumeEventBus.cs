namespace Messaging.Consume
{
    public interface IConsumeEventBus
    {
        Task ConsumeAsync<T>(string projectId, string subscriptionId, Func<T, Task> handler, CancellationToken cancellationToken);
    }
}
