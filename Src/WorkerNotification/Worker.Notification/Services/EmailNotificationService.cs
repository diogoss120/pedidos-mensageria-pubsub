using Contracts.Messages;
using System.Text;
using WorkerNotification.Services.Interfaces;

namespace WorkerNotification.Services
{
    public class EmailNotificationService(ILogger<EmailNotificationService> logger) : IEmailNotificationService
    {
        public Task NotificarAsync(PedidoCriado pedido)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Olá, {pedido.Cliente.Nome},");
            sb.AppendLine($"Recebemos o seu pedido {pedido.PedidoId} no valor total de R$ {pedido.ValorTotal:N2}.");
            sb.AppendLine("Itens do pedido:");

            foreach (var item in pedido.Itens)
            {
                sb.AppendLine($"- {item.Nome} (x{item.Quantidade}) - R$ {item.Preco:N2}");
            }

            sb.AppendLine();
            sb.AppendLine("Estamos processando o pagamento e te enviaremos outro e-mail assim que ele for aprovado.");
            sb.AppendLine();
            sb.AppendLine("Atenciosamente,");
            sb.AppendLine("Equipe EventDrivenOrders");

            logger.LogWarning("\n\n{EmailContent}", sb.ToString());

            return Task.CompletedTask;
        }
    }
}
