using Contracts.Messages;
using System.Text;
using WorkerNotification.Services.Interfaces;

namespace WorkerNotification.Services
{
    public class EmailNotificationService(ILogger<EmailNotificationService> logger) : IEmailNotificationService
    {
        public Task<string> NotificarAsync(PedidoCriado pedido)
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

            return Task.FromResult(sb.ToString());
        }

        public Task<string> NotificarAsync(PagamentoProcessado pedido)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Olá,");
            sb.AppendLine($"O pagamento do pedido {pedido.PedidoId} foi processado.");
            sb.AppendLine($"Valor: R$ {pedido.Valor:N2}");
            sb.AppendLine($"Status: {pedido.Status}");
            sb.AppendLine($"Data: {pedido.DataProcessamento}");

            if (!string.IsNullOrWhiteSpace(pedido.Detalhes))
            {
                sb.AppendLine($"Detalhes: {pedido.Detalhes}");
            }

            sb.AppendLine();
            sb.AppendLine("Atenciosamente,");
            sb.AppendLine("Equipe EventDrivenOrders");

            logger.LogWarning("\n\n{EmailContent}", sb.ToString());

            return Task.FromResult(sb.ToString());
        }

        public Task<string> NotificarAsync(PedidoDespachado pedido)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Olá,");
            sb.AppendLine($"O pedido {pedido.PedidoId} foi despachado!");
            sb.AppendLine($"Código de Rastreio: {pedido.CodigoRastreio}");
            sb.AppendLine($"Data de Envio: {pedido.DataEnvio}");

            sb.AppendLine();
            sb.AppendLine("Você pode acompanhar a entrega pelo site dos Correios.");
            sb.AppendLine();
            sb.AppendLine("Atenciosamente,");
            sb.AppendLine("Equipe EventDrivenOrders");

            logger.LogWarning("\n\n{EmailContent}", sb.ToString());

            return Task.FromResult(sb.ToString());
        }
    }
}
