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
        var indexKeys = Builders<Payment>.IndexKeys.Ascending(p => p.PedidoId);
        var indexOptions = new CreateIndexOptions { Unique = true };
        _paymentsCollection.Indexes.CreateOne(new CreateIndexModel<Payment>(indexKeys, indexOptions));
    }

    public async Task CreateAsync(Payment payment)
    {
        try
        {
            await _paymentsCollection.InsertOneAsync(payment);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Pagamento já existe, ignorar erro para garantir idempotência
        }
    }

    public async Task<Payment?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _paymentsCollection.Find(p => p.PedidoId == pedidoId).FirstOrDefaultAsync();
    }
}
