using Order.Api.Data.Entities;
using Order.Api.Data.Enum;
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
    }
}
