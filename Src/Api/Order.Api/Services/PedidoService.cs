using Order.Api.Data.Repositories.Interfaces;
using Order.Api.Dtos;
using Order.Api.Services.Interfaces;
using Order.Api.Mappers;
using Messaging.Publish;
using Microsoft.Extensions.Options;
using Order.Api.Configuration;

namespace Order.Api.Services
{
    public class PedidoService(
        IPedidoRepository pedidoRepository,
        IPublishEventBus publisher,
        IOptions<PubSubConfig> pubSubConfig) : IPedidoService
    {
        public async Task ProcessarPedido(PedidoDto pedido)
        {
            var entity = pedido.ToEntity();
            await pedidoRepository.CreateAsync(entity);

            var message = entity.ToMessage();

            await publisher.Publish(pubSubConfig.Value.ProjectId,
                pubSubConfig.Value.TopicId,    
                message);
        }

        public async Task<IEnumerable<Data.Entities.Pedido>> GetAllAsync()
        {
            return await pedidoRepository.GetAllAsync();
        }

        public async Task<Data.Entities.Pedido?> GetByIdAsync(Guid id)
        {
            return await pedidoRepository.GetByIdAsync(id);
        }
    }
}
