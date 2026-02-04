using Order.Api.Data.Entities;

namespace Order.Api.Data.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task CreateAsync(Pedido pedido);
    Task<Pedido?> GetByIdAsync(Guid id);
}
