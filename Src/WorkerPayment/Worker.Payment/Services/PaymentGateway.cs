using WorkerPayment.Dtos.Request;
using WorkerPayment.Dtos.Response;
using WorkerPayment.Services.Interfaces;

namespace WorkerPayment.Services
{
    public class PaymentGateway : IPaymentGateway
    {
        public Task<PaymentResponse> ProcessarPagamento(PagamentoDto pagamento)
        {
            var result = Random.Shared.Next(0, 3);
            
            if (result == 0)
            {
                return Task.FromResult(new PaymentResponse(true, "Pagamento processado com sucesso"));
            }

            if (result == 1)
            {
                return Task.FromResult(new PaymentResponse(false, "Falha ao processar pagamento: Saldo insuficiente"));
            }

            return Task.FromException<PaymentResponse>(new TimeoutException("O gateway de pagamento não respondeu a tempo."));
        }
    }
}
