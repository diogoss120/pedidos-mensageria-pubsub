using Order.Api.Dtos;

namespace Order.Api.Services.Interfaces
{
    public interface IPedidoService
    {
        Task ProcessarPedido(PedidoDto pedido);
    }
}
