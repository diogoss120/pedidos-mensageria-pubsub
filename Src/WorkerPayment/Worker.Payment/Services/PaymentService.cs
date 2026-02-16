using Contracts.Messages;

using WorkerPayment.Data.Entities;
using WorkerPayment.Data.Repositories.Interfaces;
using WorkerPayment.Dtos.Request;
using WorkerPayment.Dtos.Response;
using WorkerPayment.Services.Interfaces;

namespace WorkerPayment.Services
{
    public class PaymentService(
        ILogger<PaymentService> logger,
        IPaymentGateway paymentGateway,
        IPaymentRepository paymentRepository) : IPaymentService
    {
        public async Task ProcessarPagamentoAsync(PedidoCriado pedidoCriado)
        {
            logger.LogInformation("Iniciando processamento de pagamento do pedido {pedidoId}", pedidoCriado.PedidoId);

            var pagamento = new PagamentoDto(
                pedidoCriado.Pagamento.NumeroCartao,
                pedidoCriado.Pagamento.Titular,
                pedidoCriado.Pagamento.Validade,
                pedidoCriado.Pagamento.Cvv,
                pedidoCriado.Itens.Sum(i => i.Quantidade * i.Preco));

            var result = await paymentGateway.ProcessarPagamento(pagamento);

            var payment = new Payment
            {
                PedidoId = pedidoCriado.PedidoId,
                Valor = pedidoCriado.Itens.Sum(i => i.Quantidade * i.Preco),
                Status = result.Resultado ? "Aprovado" : "Recusado",
                Detalhes = result.Detalhe,
                DataProcessamento = DateTime.Now
            };

            await paymentRepository.CreateAsync(payment);

            logger.LogInformation("Pagamento do pedido {pedidoId} processado com sucesso", pedidoCriado.PedidoId);

            // TODO: Postar evento do resultado final (PagamentoAprovado ou PagamentoRecusado)
            // tanto o WorkerNotification quanto o WorkerInventory irão consumir esse evento, e tomar as ações necessárias.
        }
    }
}
