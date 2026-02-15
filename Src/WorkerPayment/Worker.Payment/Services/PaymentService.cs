using Contracts.Messages;
using Polly;
using Polly.Fallback;
using Polly.Retry;
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
        // Pipeline estático para evitar recriação a cada requisiçãoon)
        private static readonly ResiliencePipeline<PaymentResponse> _pipeline = CreatePipeline();

        public async Task ProcessarPagamentoAsync(PedidoCriado pedidoCriado)
        {
            logger.LogInformation("Iniciando processamento de pagamento do pedido {pedidoId}", pedidoCriado.PedidoId);

            var pagamento = new PagamentoDto(
                pedidoCriado.Pagamento.NumeroCartao,
                pedidoCriado.Pagamento.Titular,
                pedidoCriado.Pagamento.Validade,
                pedidoCriado.Pagamento.Cvv,
                pedidoCriado.Itens.Sum(i => i.Quantidade * i.Preco));

            var result = await _pipeline.ExecuteAsync(async token =>
            {
                return await paymentGateway.ProcessarPagamento(pagamento);
            });

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

        private static ResiliencePipeline<PaymentResponse> CreatePipeline()
        {
            // Configuração do Fallback
            var fallbackOptions = new FallbackStrategyOptions<PaymentResponse>
            {
                ShouldHandle = new PredicateBuilder<PaymentResponse>()
                    .Handle<HttpRequestException>() 
                    .Handle<TimeoutException>()
                    .Handle<TaskCanceledException>(),

                FallbackAction = args =>
                {
                    return ValueTask.FromResult(Outcome.FromResult(new PaymentResponse(false, "Erro técnico persistente (Gateway fora do ar). Favor processar manualmente.")));
                }
            };

            // Configuração do Retry
            var retryOptions = new RetryStrategyOptions<PaymentResponse>
            {
                ShouldHandle = new PredicateBuilder<PaymentResponse>()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutException>(), 
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 3,
                UseJitter = true,
                Delay = TimeSpan.FromSeconds(2),
                OnRetry = args =>
                {
                    Console.WriteLine($"Tentativa {args.AttemptNumber}. Erro: {args.Outcome.Exception?.Message}");
                    return default;
                }
            };

            // Montagem do Pipeline
            return new ResiliencePipelineBuilder<PaymentResponse>()
                .AddFallback(fallbackOptions)
                .AddRetry(retryOptions)
                .Build();
        }
    }
}
