using WorkerShipping.Data.Entities;

namespace WorkerShipping.Data.Repositories.Interfaces;

public interface IEnvioRepository
{
    Task CreateAsync(Envio envio);
    Task UpdateAsync(Envio envio);
    Task<Envio?> GetByPedidoIdAsync(Guid pedidoId);
}
