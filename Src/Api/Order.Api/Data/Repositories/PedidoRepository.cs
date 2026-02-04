using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Order.Api.Data.Entities;
using Order.Api.Data.Repositories.Interfaces;
using Order.Api.Data.Settings;

namespace Order.Api.Data.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly IMongoCollection<Pedido> _pedidosCollection;

    public PedidoRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _pedidosCollection = mongoDatabase.GetCollection<Pedido>("Pedidos");
    }

    public async Task CreateAsync(Pedido pedido)
    {
        await _pedidosCollection.InsertOneAsync(pedido);
    }

    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _pedidosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
