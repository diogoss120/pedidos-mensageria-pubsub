using Contracts.Enums;
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

            var existingPayment = await paymentRepository.GetByPedidoIdAsync(pedidoCriado.PedidoId);
            if (existingPayment != null)
            {
                if (existingPayment.Status == PaymentStatus.Processando)
                {
                    logger.LogWarning("Pagamento para o pedido {PedidoId} está em processamento.", pedidoCriado.PedidoId);
                    return;
                }
                
                logger.LogWarning("Pagamento para o pedido {PedidoId} já foi processado (Status: {Status}).", pedidoCriado.PedidoId, existingPayment.Status);
                return;
            }

            // Cria o registro inicial com status "Processando"
            var payment = new Payment
            {
                PedidoId = pedidoCriado.PedidoId,
                Valor = pedidoCriado.Itens.Sum(i => i.Quantidade * i.Preco),
                Status = PaymentStatus.Processando,
                Detalhes = "Processamento iniciado",
                DataProcessamento = DateTime.Now
            };

            try
            {
                await paymentRepository.CreateAsync(payment);
            }
            catch (Exception)
            {
                logger.LogWarning("Concorrência detectada ao criar pagamento para o pedido {PedidoId}. Abortando execução.", pedidoCriado.PedidoId);
                return;
            }

            var pagamentoDto = PaymentMapper.ToPagamentoDto(pedidoCriado);
            var result = await paymentGateway.ProcessarPagamentoAsync(pagamentoDto);

            // Atualiza o registro com o resultado final
            payment.Status = result.Status;
            payment.Detalhes = result.Detalhe;
            payment.DataProcessamento = DateTime.Now;

            await paymentRepository.UpdateAsync(payment);

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
