using MongoDB.Driver;
using WorkerPayment.Data.Entities;
using WorkerPayment.Data.Repositories.Interfaces;

namespace WorkerPayment.Data.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _paymentsCollection;

    public PaymentRepository(IMongoDatabase database)
    {
        _paymentsCollection = database.GetCollection<Payment>("Pagamentos");
    }

    public async Task CreateAsync(Payment payment)
    {
        await _paymentsCollection.InsertOneAsync(payment);
    }

    public async Task<Payment?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _paymentsCollection.Find(p => p.PedidoId == pedidoId).FirstOrDefaultAsync();
    }
}
