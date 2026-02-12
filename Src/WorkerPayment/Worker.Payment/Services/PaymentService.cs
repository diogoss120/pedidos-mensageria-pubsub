using Contracts.Messages;
using WorkerPayment.Data.Entities;
using WorkerPayment.Data.Repositories.Interfaces;
using WorkerPayment.Services.Interfaces;

namespace WorkerPayment.Services
{
    public class PaymentService(
        ILogger<PaymentService> logger,
        IPaymentRepository paymentRepository) : IPaymentService
    {
        public async Task ProcessarPagamentoAsync(PedidoCriado pedidoCriado)
        {
            logger.LogInformation("Iniciando processamento de pagamento do pedido {pedidoId}", pedidoCriado.PedidoId);

            // Simulação de processamento dos dados do cartão
            // Implementar depois um sorteio de true ou false para simular aprovação ou recusa do pagamento
            // Em caso de recusa, implemetar um retry com um número máximo de tentativas
            await Task.Delay(100); 

            var payment = new Payment
            {
                PedidoId = pedidoCriado.PedidoId,
                Valor = pedidoCriado.Itens.Sum(i => i.Quantidade * i.Preco), 
                Status = "Aprovado",
                DataProcessamento = DateTime.Now
            };

            await paymentRepository.CreateAsync(payment);

            logger.LogInformation("Pagamento do pedido {pedidoId} processado com sucesso", pedidoCriado.PedidoId);

            // TODO: Postar evento do resultado final (PagamentoAprovado ou PagamentoRecusado)
            // tanto o WorkerNotification quanto o WorkerInventory irão consumir esse evento, e tomar as ações necessárias.
        }
    }
}
