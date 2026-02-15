using Contracts.Messages;
using Messaging.Consume;
using Microsoft.Extensions.Options;
using WorkerPayment.Configuration;
using WorkerPayment.Services.Interfaces;

namespace WorkerPayment
{
    public class Worker(
        ILogger<Worker> logger,
        IConsumeEventBus consumeEventBus,
        IOptions<PubSubConfig> pubSubConfig,
        IPaymentService paymentService
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("WorkerPayment running at: {time}", DateTimeOffset.Now);

                await consumeEventBus.ConsumeAsync<PedidoCriado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionId,
                    ProcessarPagamento,
                    stoppingToken); 
            }
        }

        private async Task ProcessarPagamento(PedidoCriado criado)
        {
            await paymentService.ProcessarPagamentoAsync(criado);
        }
    }
}
