using Contracts.Messages;
using WorkerShipping.Data.Repositories.Interfaces;
using WorkerShipping.Mappers;
using WorkerShipping.Services.Interfaces;

using WorkerShipping.Data.Enums;
using WorkerShipping.Services;

namespace WorkerShipping.Services;

public class EnvioService(
    ILogger<EnvioService> logger,
    IEnvioRepository envioRepository,
    ICorreiosGateway correiosGateway) : IEnvioService
{
    public async Task ProcessarEnvioAsync(PagamentoProcessado pagamento)
    {
        logger.LogInformation("Iniciando processamento de envio para o pedido {PedidoId}", pagamento.PedidoId);

        var envioDto = EnvioMapper.ToEnvioDto(pagamento);

        var response = await correiosGateway.ProcessarEnvioAsync(envioDto);

        var envio = EnvioMapper.ToEnvioEntity(pagamento, response);

        await envioRepository.CreateAsync(envio);

        if (response.Status == ShippingStatus.Despachado)
        {
            logger.LogInformation("Envio do pedido {PedidoId} criado com código de rastreio {CodigoRastreio}", pagamento.PedidoId, envio.CodigoRastreio);
        }
        else
        {
            logger.LogWarning("Falha no envio do pedido {PedidoId}: {Mensagem}", pagamento.PedidoId, response.Mensagem);
        }
    }
}
