using Order.Api.Data.Repositories.Interfaces;
using Order.Api.Dtos;
using Order.Api.Services.Interfaces;
using Order.Api.Mappers;

namespace Order.Api.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task ProcessarPedido(PedidoDto pedido)
        {
            var entity = pedido.ToEntity();
            await _pedidoRepository.CreateAsync(entity);

            // preciso enviar o evento do pedido criado
            var message = entity.ToMessage();
        }

        public async Task<IEnumerable<Data.Entities.Pedido>> GetAllAsync()
        {
            return await _pedidoRepository.GetAllAsync();
        }

        public async Task<Data.Entities.Pedido?> GetByIdAsync(Guid id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }
    }
}
