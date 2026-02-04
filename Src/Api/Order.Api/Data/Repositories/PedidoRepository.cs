using MongoDB.Driver;
using Order.Api.Data.Entities;
using Order.Api.Data.Repositories.Interfaces;

namespace Order.Api.Data.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly IMongoCollection<Pedido> _pedidosCollection;

    public PedidoRepository(IMongoDatabase database)
    {
        _pedidosCollection = database.GetCollection<Pedido>("Pedidos");
    }

    public async Task CreateAsync(Pedido pedido)
    {
        await _pedidosCollection.InsertOneAsync(pedido);
    }

    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _pedidosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        return await _pedidosCollection.Find(_ => true).ToListAsync();
    }
}
