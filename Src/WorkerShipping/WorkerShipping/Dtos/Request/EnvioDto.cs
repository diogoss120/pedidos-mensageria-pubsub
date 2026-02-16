namespace WorkerShipping.Dtos.Request;

public record EnvioDto(
    Guid PedidoId,
    string Endereco,
    string NomeDestinatario
);
