using Contracts.Messages;

namespace WorkerNotification.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        Task<string> NotificarAsync(PedidoCriado pedido);
        Task<string> NotificarAsync(PagamentoProcessado pedido);
    }
}
