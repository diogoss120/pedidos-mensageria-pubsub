using Contracts.Messages;
using Messaging.Consume;
using Microsoft.Extensions.Options;
using WorkerNotification.Configuration;
using WorkerNotification.Services.Interfaces;

namespace WorkerNotification
{
    public class Worker(
        ILogger<Worker> logger,
        IConsumeEventBus consumeEventBus,
        IOptions<PubSubConfig> pubSubConfig,
        INotificationService notificationService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("WorkerNotification running at: {time}", DateTimeOffset.Now);

                await consumeEventBus.ConsumeAsync<PedidoCriado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionId,
                    NotificarPedidoCriado,
                    stoppingToken);
            }
        }

        private async Task NotificarPedidoCriado(PedidoCriado message)
        {
            await notificationService.ProcessarPedidoCriadoAsync(message);
        }
    }
}
