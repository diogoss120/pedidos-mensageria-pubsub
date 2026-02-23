using Contracts.Messages;
using Messaging.Consume;
using Microsoft.Extensions.Options;
using WorkerShipping.Configuration;
using WorkerShipping.Services.Interfaces;

namespace WorkerShipping
{
    public class Worker(
        ILogger<Worker> logger,
        IConsumeEventBus consumeEventBus,
        IOptions<PubSubConfig> pubSubConfig,
        IEnvioService envioService
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("WorkerShipping running at: {time}", DateTimeOffset.Now);

                await consumeEventBus.ConsumeAsync<PagamentoProcessado>(
                    pubSubConfig.Value.ProjectId,
                    pubSubConfig.Value.SubscriptionId,
                    ProcessarEnvio,
                    stoppingToken);
            }
        }

        private async Task ProcessarEnvio(PagamentoProcessado message)
        {
            await envioService.ProcessarEnvioAsync(message);
        }
    }
}
