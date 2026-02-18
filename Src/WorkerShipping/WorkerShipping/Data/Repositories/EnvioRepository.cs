using MongoDB.Driver;
using WorkerShipping.Data.Entities;
using WorkerShipping.Data.Repositories.Interfaces;

namespace WorkerShipping.Data.Repositories;

public class EnvioRepository : IEnvioRepository
{
    private readonly IMongoCollection<Envio> _enviosCollection;

    public EnvioRepository(IMongoDatabase database)
    {
        _enviosCollection = database.GetCollection<Envio>("Envios");
    }

    public async Task CreateAsync(Envio envio)
    {
        await _enviosCollection.InsertOneAsync(envio);
    }

    public async Task<Envio?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _enviosCollection.Find(p => p.PedidoId == pedidoId).FirstOrDefaultAsync();
    }
}
