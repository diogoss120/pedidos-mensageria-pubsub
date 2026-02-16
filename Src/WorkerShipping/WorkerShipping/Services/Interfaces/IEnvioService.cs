using Contracts.Messages;

namespace WorkerShipping.Services.Interfaces;

public interface IEnvioService
{
    Task ProcessarEnvioAsync(PagamentoProcessado pagamento);
}
