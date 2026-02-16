using Contracts.Messages;
using WorkerShipping.Data.Entities;
using WorkerShipping.Dtos.Request;
using WorkerShipping.Dtos.Response;

namespace WorkerShipping.Mappers;

public static class EnvioMapper
{
    public static EnvioDto ToEnvioDto(PagamentoProcessado pagamento)
    {
        // Como o evento de PagamentoProcessado não possui dados do cliente/endereço,
        // vamos simular esses dados para fins de demonstração.
        // Em um cenário real, poderíamos buscar esses dados no banco de pedidos.
        return new EnvioDto(
            pagamento.PedidoId,
            "Rua Exemplo, 123 - Cidade/UF",
            "Cliente Exemplo"
        );
    }

    public static Envio ToEnvioEntity(PagamentoProcessado pagamento, EnvioResponse resultado)
    {
        return new Envio
        {
            Id = Guid.NewGuid(),
            PedidoId = pagamento.PedidoId,
            CodigoRastreio = resultado.TrackingCode ?? string.Empty,
            Status = resultado.Sucesso ? "Enviado" : "Falha",
            Detalhes = resultado.Mensagem,
            DataPostagem = DateTime.UtcNow
        };
    }
}
