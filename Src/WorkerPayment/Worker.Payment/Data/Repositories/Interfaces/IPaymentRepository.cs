using WorkerPayment.Data.Entities;

namespace WorkerPayment.Data.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task CreateAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task<Payment?> GetByPedidoIdAsync(Guid pedidoId);
}
