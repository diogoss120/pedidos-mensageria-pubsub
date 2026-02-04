using Order.Api.Data.Entities;
using Contracts.Enums;
using Order.Api.Dtos;

namespace Order.Api.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido ToEntity(this PedidoDto dto)
        {
            return new Pedido
            {
                Id = Guid.NewGuid(),
                CriadoEm = DateTime.UtcNow,
                Status = StatusPedido.PROCESSANDO,
                Cliente = new Cliente
                {
                    Nome = dto.Cliente.Nome,
                    Email = dto.Cliente.Email
                },
                Itens = dto.Itens.Select(i => new ItemPedido
                {
                    Nome = i.Nome,
                    Quantidade = i.Quantidade,
                    Preco = i.Preco
                }).ToList(),
                Pagamento = new Pagamento
                {
                    NumeroCartao = dto.Pagamento.NumeroCartao,
                    Titular = dto.Pagamento.Titular,
                    Expiracao = dto.Pagamento.Validade,
                    Cvv = dto.Pagamento.Cvv
                }
            };
        }

        public static Contracts.Messages.PedidoCriado ToMessage(this Pedido entity)
        {
            return new Contracts.Messages.PedidoCriado
            {
                CorrelationId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                PedidoId = entity.Id,
                Status = entity.Status.ToString(),
                ValorTotal = entity.Total,
                Cliente = new Contracts.Messages.Cliente
                {
                    Nome = entity.Cliente.Nome,
                    Email = entity.Cliente.Email
                },
                Itens = entity.Itens.Select(i => new Contracts.Messages.ItemPedido
                {
                    Nome = i.Nome,
                    Quantidade = i.Quantidade,
                    Preco = i.Preco
                }).ToList(),
                Pagamento = new Contracts.Messages.Pagamento
                {
                    NumeroCartao = entity.Pagamento.NumeroCartao,
                    Titular = entity.Pagamento.Titular,
                    Validade = entity.Pagamento.Expiracao,
                    Cvv = entity.Pagamento.Cvv
                }
            };
        }
    }
}
