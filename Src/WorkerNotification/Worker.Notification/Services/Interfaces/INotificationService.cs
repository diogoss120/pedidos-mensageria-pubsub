using Contracts.Messages;

namespace WorkerNotification.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotificarPedidoCriadoAsync(PedidoCriado message);

        Task NotificarPagamentoProcessadoAsync(PagamentoProcessado message);
    }
}
