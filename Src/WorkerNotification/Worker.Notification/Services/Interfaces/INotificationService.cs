using Contracts.Messages;

namespace WorkerNotification.Services.Interfaces
{
    public interface INotificationService
    {
        Task ProcessarPedidoCriadoAsync(PedidoCriado message);
    }
}
