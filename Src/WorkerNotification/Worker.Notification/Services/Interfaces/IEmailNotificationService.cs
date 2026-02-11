using Contracts.Messages;

namespace WorkerNotification.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        Task NotificarAsync(PedidoCriado pedido);
    }
}
