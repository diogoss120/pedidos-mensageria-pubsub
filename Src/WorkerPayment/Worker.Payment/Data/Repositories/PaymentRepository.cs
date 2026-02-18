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
            // Pagamento já existe, lançar exceção ou retornar false seria ideal, 
            // mas manteremos o comportamento de ignorar por enquanto, 
            // pois o Service fará a verificação antes.
            // Para o fluxo "Processando", a falha aqui indica concorrência.
            throw new InvalidOperationException("Pagamento duplicado detectado durante a criação.");
        }
    }

    public async Task UpdateAsync(Payment payment)
    {
        await _paymentsCollection.ReplaceOneAsync(p => p.PedidoId == payment.PedidoId, payment);
    }

    public async Task<Payment?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _paymentsCollection.Find(p => p.PedidoId == pedidoId).FirstOrDefaultAsync();
    }
}
