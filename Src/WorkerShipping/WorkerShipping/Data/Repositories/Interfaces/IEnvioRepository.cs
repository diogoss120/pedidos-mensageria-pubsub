using WorkerShipping.Data.Entities;

namespace WorkerShipping.Data.Repositories.Interfaces;

public interface IEnvioRepository
{
    Task CreateAsync(Envio envio);
}
