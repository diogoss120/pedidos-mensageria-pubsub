using Contracts.Messages;
using Messaging.Publish;
using Microsoft.Extensions.Options;
using WorkerPayment.Configuration;
using WorkerPayment.Data.Entities;
using WorkerPayment.Data.Repositories.Interfaces;
using WorkerPayment.Mappers;
using WorkerPayment.Services.Interfaces;

namespace WorkerPayment.Services
{
    public class PaymentService(
        ILogger<PaymentService> logger,
        IPaymentGateway paymentGateway,
        IPublishEventBus publishEventBus,
        IOptions<PubSubConfig> pubSubConfig,
        IPaymentRepository paymentRepository) : IPaymentService
    {
        public async Task ProcessarPagamentoAsync(PedidoCriado pedidoCriado)
        {
            logger.LogInformation("Iniciando processamento de pagamento do pedido {pedidoId}", pedidoCriado.PedidoId);

            var pagamento = PaymentMapper.ToPagamentoDto(pedidoCriado);

            var result = await paymentGateway.ProcessarPagamentoAsync(pagamento);

            var payment = PaymentMapper.ToPaymentEntity(pedidoCriado, result);

            await paymentRepository.CreateAsync(payment);

            await PublishPagamentoProcessadoAsync(payment);

            logger.LogInformation("Pagamento do pedido {pedidoId} foi {Resultado}", pedidoCriado.PedidoId, payment.Status);
        }

        private async Task PublishPagamentoProcessadoAsync(Payment payment)
        {
            var evento = PaymentMapper.ToPagamentoProcessadoEvent(payment);
            await publishEventBus.Publish(
                pubSubConfig.Value.ProjectId,
                pubSubConfig.Value.TopicId,
                evento);
        }
    }
}
