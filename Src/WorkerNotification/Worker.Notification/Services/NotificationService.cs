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
        public async Task NotificarPedidoCriadoAsync(PedidoCriado message)
        {
            logger.LogInformation("Pedido {pedidoId} foi criado com sucesso", message.PedidoId);

            var notificacao = await emailNotificationService.NotificarAsync(message);

            await notificationRepository.CreateAsync(new Notification
            {
                PedidoId = message.PedidoId,
                ConteudoEmail = notificacao,
                DataEnvio = DateTime.Now
            });
        }

        public async Task NotificarPagamentoProcessadoAsync(PagamentoProcessado message)
        {
            logger.LogInformation("O pagamento do pedido {pedidoId} foi {Resultado}", message.PedidoId, message.Status);

            var notificacao = await emailNotificationService.NotificarAsync(message);

            await notificationRepository.CreateAsync(new Notification
            {
                PedidoId = message.PedidoId,
                ConteudoEmail = notificacao,
                DataEnvio = DateTime.Now
            });
        }

        public async Task NotificarPedidoDespachadoAsync(PedidoDespachado message)
        {
            logger.LogInformation("O pedido {pedidoId} foi enviado com o código {CodigoRastreio}", message.PedidoId, message.CodigoRastreio);

            var notificacao = await emailNotificationService.NotificarAsync(message);

            await notificationRepository.CreateAsync(new Notification
            {
                PedidoId = message.PedidoId,
                ConteudoEmail = notificacao,
                DataEnvio = DateTime.Now
            });
        }

    }
}
