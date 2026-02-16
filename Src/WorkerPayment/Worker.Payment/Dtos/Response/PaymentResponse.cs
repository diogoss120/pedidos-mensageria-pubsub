using WorkerPayment.Data.Enums;

namespace WorkerPayment.Dtos.Response
{
    public record PaymentResponse(PaymentStatus Status, string Detalhe);
}
