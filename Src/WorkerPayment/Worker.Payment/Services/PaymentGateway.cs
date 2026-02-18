using WorkerPayment.Dtos.Request;
using WorkerPayment.Dtos.Response;
using WorkerPayment.Services.Interfaces;
using Polly;
using Polly.Fallback;
using Polly.Retry;
using Contracts.Enums;

namespace WorkerPayment.Services
{
    public class PaymentGateway : IPaymentGateway
    {
        // Pipeline estático para evitar recriação a cada requisição
        private static readonly ResiliencePipeline<PaymentResponse> _pipeline = CreatePipeline();

        public async Task<PaymentResponse> ProcessarPagamentoAsync(PagamentoDto pagamento)
        {
            return await _pipeline.ExecuteAsync(async token =>
            {
                // Simulação da chamada externa
                var result = Random.Shared.Next(0, 3);

                if (result == 0)
                {
                    return new PaymentResponse(PaymentStatus.Aprovado, "Pagamento processado com sucesso");
                }

                if (result == 1)
                {
                    return new PaymentResponse(PaymentStatus.Recusado, "Falha ao processar pagamento: Saldo insuficiente");
                }

                // Simula um timeout
                throw new TimeoutException("O gateway de pagamento não respondeu a tempo.");
            });
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
                    return ValueTask.FromResult(Outcome.FromResult(new PaymentResponse(PaymentStatus.ErroTecnico, "Erro técnico persistente (Gateway fora do ar). Favor processar manualmente.")));
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
