using Contracts.Messages;
using WorkerNotification.Data.Entities;
using WorkerNotification.Data.Repositories.Interfaces;
using WorkerNotification.Services.Interfaces;

namespace WorkerNotification.Services
{
    public class NotificationService(
        ILogger<NotificationService> logger,
        IEmailNotificationService emailNotificationService,
        INotificationRepository notificationRepository) : INotificationService
    {
        public async Task ProcessarPedidoCriadoAsync(PedidoCriado message)
        {
            logger.LogInformation("Pedido {pedidoId} foi criado com sucesso", message.PedidoId);
            
            await emailNotificationService.NotificarAsync(message);
            
            await notificationRepository.CreateAsync(new Notification
            {
                PedidoId = message.PedidoId,
                ClienteNome = message.Cliente.Nome,
                ConteudoEmail = $"Pedido {message.PedidoId} criado com sucesso - notificação enviada ao cliente",
                DataEnvio = DateTime.Now
            });
        }
    }
}
