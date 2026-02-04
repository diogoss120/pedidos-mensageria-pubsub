using Order.Api.Dtos;

namespace Order.Api.Services.Interfaces
{
    public interface IPedidoService
    {
        Task ProcessarPedido(PedidoDto pedido);
        Task<IEnumerable<Order.Api.Data.Entities.Pedido>> GetAllAsync();
        Task<Order.Api.Data.Entities.Pedido?> GetByIdAsync(Guid id);
    }
}
