using Polly;
using Polly.Fallback;
using Polly.Retry;
using WorkerShipping.Data.Enums;
using WorkerShipping.Dtos.Request;
using WorkerShipping.Dtos.Response;
using WorkerShipping.Services.Interfaces;

namespace WorkerShipping.Services;

public class CorreiosGateway : ICorreiosGateway
{
    private static readonly ResiliencePipeline<EnvioResponse> _pipeline = CreatePipeline();

    public async Task<EnvioResponse> ProcessarEnvioAsync(EnvioDto envio)
    {
        return await _pipeline.ExecuteAsync(async token =>
        {
            // Sorteio entre sucesso (0) e timeout (1)
            var result = Random.Shared.Next(0, 2);

            if (result == 0)
            {
                var trackingCode = $"BR{envio.PedidoId.ToString().Substring(0, 8).ToUpper()}Correios";
                return new EnvioResponse(ShippingStatus.Despachado, trackingCode, "Envio registrado nos Correios com sucesso.");
            }

            // Simula um timeout
            throw new TimeoutException("O sistema dos Correios não respondeu a tempo.");
        });
    }

    private static ResiliencePipeline<EnvioResponse> CreatePipeline()
    {
        // Configuração do Fallback
        var fallbackOptions = new FallbackStrategyOptions<EnvioResponse>
        {
            ShouldHandle = new PredicateBuilder<EnvioResponse>()
                .Handle<HttpRequestException>()
                .Handle<TimeoutException>()
                .Handle<TaskCanceledException>(),

            FallbackAction = args =>
            {
                return ValueTask.FromResult(Outcome.FromResult(new EnvioResponse(ShippingStatus.ErroTecnico, string.Empty, "Falha na comunicação com os Correios. Tente novamente mais tarde.")));
            }
        };

        // Configuração do Retry
        var retryOptions = new RetryStrategyOptions<EnvioResponse>
        {
            ShouldHandle = new PredicateBuilder<EnvioResponse>()
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
        return new ResiliencePipelineBuilder<EnvioResponse>()
            .AddFallback(fallbackOptions)
            .AddRetry(retryOptions)
            .Build();
    }
}
