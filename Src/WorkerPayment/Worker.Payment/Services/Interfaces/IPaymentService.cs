using Contracts.Messages;

namespace WorkerPayment.Services.Interfaces;

public interface IPaymentService
{
    Task ProcessarPagamentoAsync(PedidoCriado pedidoCriado);
}
