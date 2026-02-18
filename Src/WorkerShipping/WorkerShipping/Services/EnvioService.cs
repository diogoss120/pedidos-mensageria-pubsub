using Contracts.Enums;
using Contracts.Messages;
using Messaging.Publish;
using Microsoft.Extensions.Options;
using WorkerShipping.Configuration;
using WorkerShipping.Data.Entities;
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
            if (existingEnvio.Status == ShippingStatus.Processando)
            {
                logger.LogWarning("Envio para o pedido {PedidoId} está em processamento.", pagamento.PedidoId);
                return;
            }

            logger.LogWarning("Envio para o pedido {PedidoId} já foi processado (Status: {Status}).", pagamento.PedidoId, existingEnvio.Status);
            return;
        }

        if (pagamento.Status != PaymentStatus.Aprovado.ToString())
        {
            logger.LogInformation("Pedido {PedidoId} não está aprovado. Não será processado para envio.", pagamento.PedidoId);
            return;
        }

        // Cria o registro inicial com status "Processando"
        var envio = new Envio
        {
            PedidoId = pagamento.PedidoId,
            Status = ShippingStatus.Processando,
            Detalhes = "Processamento de envio iniciado",
            DataPostagem = DateTime.UtcNow
        };

        try
        {
            await envioRepository.CreateAsync(envio);
        }
        catch (Exception)
        {
            logger.LogWarning("Concorrência detectada ao criar envio para o pedido {PedidoId}. Abortando execução.", pagamento.PedidoId);
            return;
        }

        var envioDto = EnvioMapper.ToEnvioDto(pagamento);

        var response = await correiosGateway.ProcessarEnvioAsync(envioDto);

        // Atualiza o registro com o resultado final
        envio.CodigoRastreio = response.TrackingCode ?? string.Empty;
        envio.Status = response.Status;
        envio.Detalhes = response.Mensagem;
        envio.DataPostagem = DateTime.UtcNow;

        await envioRepository.UpdateAsync(envio);

        if (response.Status == ShippingStatus.Despachado)
        {
            logger.LogInformation("Envio do pedido {PedidoId} criado com código de rastreio {CodigoRastreio}", pagamento.PedidoId, envio.CodigoRastreio);

            await PublicarPedidoDespachadoAsync(pagamento, envio);
        }
        else
        {
            logger.LogWarning("Falha no envio do pedido {PedidoId}: {Mensagem}", pagamento.PedidoId, response.Mensagem);
        }
    }

    private async Task PublicarPedidoDespachadoAsync(PagamentoProcessado pagamento, Envio envio)
    {
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
}
