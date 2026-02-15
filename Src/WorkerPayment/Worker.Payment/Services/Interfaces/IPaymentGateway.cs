using WorkerPayment.Dtos.Request;
using WorkerPayment.Dtos.Response;

namespace WorkerPayment.Services.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResponse> ProcessarPagamento(PagamentoDto pagamento);
    }
}
