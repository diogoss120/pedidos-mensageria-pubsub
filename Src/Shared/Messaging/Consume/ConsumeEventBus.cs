using Google.Cloud.PubSub.V1;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Messaging.Consume
{
    public class ConsumeEventBus : IConsumeEventBus
    {
        private readonly ConcurrentDictionary<string, SubscriberClient> _subscribers = new();

        public async Task ConsumeAsync<T>(
            string projectId,
            string subscriptionId,
            Func<T, Task> handler,
            CancellationToken cancellationToken
        )
        {
            var key = $"{projectId}:{subscriptionId}";

            var subscriber = _subscribers.GetOrAdd(
                key,
                _ => CreateSubscriber(projectId, subscriptionId)
                        .GetAwaiter()
                        .GetResult()
            );

            await subscriber.StartAsync(async (message, cancel) =>
            {
                try
                {
                    var json = message.Data.ToStringUtf8();
                    var payload = JsonSerializer.Deserialize<T>(json);

                    if (payload is null)
                        throw new InvalidOperationException("Mensagem inválida");

                    await handler(payload);

                    return SubscriberClient.Reply.Ack;
                }
                catch
                {
                    return SubscriberClient.Reply.Nack;
                }
            });

            await Task.Delay(Timeout.Infinite, cancellationToken);

            await subscriber.StopAsync(cancellationToken);
        }

        private static async Task<SubscriberClient> CreateSubscriber(
            string projectId,
            string subscriptionId
        )
        {
            var subscriptionName =
                SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

            return await SubscriberClient.CreateAsync(subscriptionName);
        }
    }
}
