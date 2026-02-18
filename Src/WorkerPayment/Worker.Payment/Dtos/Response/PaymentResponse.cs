
using Contracts.Enums;

namespace WorkerPayment.Dtos.Response
{
    public record PaymentResponse(PaymentStatus Status, string Detalhe);
}
