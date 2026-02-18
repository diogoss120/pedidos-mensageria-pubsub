using Contracts.Messages;
using Messaging.Publish;
using Microsoft.Extensions.Options;
using WorkerShipping.Configuration;
using WorkerShipping.Data.Enums;
using WorkerShipping.Data.Repositories.Interfaces;
using WorkerShipping.Mappers;
using WorkerShipping.Services.Interfaces;

namespace WorkerShipping.Services;

public class EnvioService(
    ILogger<EnvioService> logger,
    IEnvioRepository envioRepository,
    ICorreiosGateway correiosGateway,
    IPublishEventBus publishEventBus,
    IOptions<PubSubConfig> pubSubConfig) : IEnvioService
{
    public async Task ProcessarEnvioAsync(PagamentoProcessado pagamento)
    {
        logger.LogInformation("Iniciando processamento de envio para o pedido {PedidoId}", pagamento.PedidoId);

        var existingEnvio = await envioRepository.GetByPedidoIdAsync(pagamento.PedidoId);
        if (existingEnvio != null)
        {
            logger.LogWarning("Envio para o pedido {PedidoId} já foi processado.", pagamento.PedidoId);
            return;
        }

        var envioDto = EnvioMapper.ToEnvioDto(pagamento);

        var response = await correiosGateway.ProcessarEnvioAsync(envioDto);

        var envio = EnvioMapper.ToEnvioEntity(pagamento, response);

        await envioRepository.CreateAsync(envio);

        if (response.Status == ShippingStatus.Despachado)
        {
            logger.LogInformation("Envio do pedido {PedidoId} criado com código de rastreio {CodigoRastreio}", pagamento.PedidoId, envio.CodigoRastreio);

            var pedidoDespachado = new PedidoDespachado
            {
                PedidoId = pagamento.PedidoId,
                CodigoRastreio = envio.CodigoRastreio,
                DataEnvio = envio.DataPostagem
            };

            await publishEventBus.Publish(
                pubSubConfig.Value.ProjectId,
                pubSubConfig.Value.TopicId,
                pedidoDespachado);
        }
        else
        {
            logger.LogWarning("Falha no envio do pedido {PedidoId}: {Mensagem}", pagamento.PedidoId, response.Mensagem);
        }
    }
}
