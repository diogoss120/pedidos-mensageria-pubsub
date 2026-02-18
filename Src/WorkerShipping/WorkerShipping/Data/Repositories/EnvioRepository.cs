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
        var indexKeys = Builders<Envio>.IndexKeys.Ascending(p => p.PedidoId);
        var indexOptions = new CreateIndexOptions { Unique = true };
        _enviosCollection.Indexes.CreateOne(new CreateIndexModel<Envio>(indexKeys, indexOptions));
    }

    public async Task CreateAsync(Envio envio)
    {
        try
        {
            await _enviosCollection.InsertOneAsync(envio);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Envio já existe, lançar exceção para permitir controle de concorrência no Service
            throw new InvalidOperationException("Envio duplicado detectado durante a criação.");
        }
    }

    public async Task UpdateAsync(Envio envio)
    {
        await _enviosCollection.ReplaceOneAsync(e => e.PedidoId == envio.PedidoId, envio);
    }

    public async Task<Envio?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _enviosCollection.Find(p => p.PedidoId == pedidoId).FirstOrDefaultAsync();
    }
}
