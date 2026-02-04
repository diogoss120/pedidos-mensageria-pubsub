using Order.Api.Dtos;
using Order.Api.Services.Interfaces;

namespace Order.Api.Services
{
    public class PedidoService : IPedidoService
    {
        public async Task ProcessarPedido(PedidoDto pedido)
        {
            // preciso gravar o pedido no banco


            // preciso enviar o evento do pedido criado


            throw new NotImplementedException();
        }
    }
}
