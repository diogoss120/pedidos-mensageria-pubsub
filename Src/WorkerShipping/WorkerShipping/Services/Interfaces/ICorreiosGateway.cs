using WorkerShipping.Dtos.Request;
using WorkerShipping.Dtos.Response;

namespace WorkerShipping.Services.Interfaces;

public interface ICorreiosGateway
{
    Task<EnvioResponse> ProcessarEnvioAsync(EnvioDto envio);
}
