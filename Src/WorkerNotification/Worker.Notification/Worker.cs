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

                var taskPedidoCriado = consumeEventBus.ConsumeAsync<PedidoCriado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionIdPedidoCriado,
                    NotificarPedidoCriado,
                    stoppingToken);

                var taskPagamentoProcessado = consumeEventBus.ConsumeAsync<PagamentoProcessado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionIdPagamentoProcessado,
                    NotificarPagamentoProcessado,
                    stoppingToken);

                var taskPedidoDespachado = consumeEventBus.ConsumeAsync<PedidoDespachado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionIdPedidoDespachado,
                    NotificarPedidoDespachado,
                    stoppingToken);

                await Task.WhenAll(taskPedidoCriado, taskPagamentoProcessado, taskPedidoDespachado);
            }
        }

        private async Task NotificarPedidoCriado(PedidoCriado message)
        {
            await notificationService.NotificarPedidoCriadoAsync(message);
        }

        private async Task NotificarPagamentoProcessado(PagamentoProcessado message)
        {
            await notificationService.NotificarPagamentoProcessadoAsync(message);
        }

        private async Task NotificarPedidoDespachado(PedidoDespachado message)
        {
            await notificationService.NotificarPedidoDespachadoAsync(message);
        }
    }
}
