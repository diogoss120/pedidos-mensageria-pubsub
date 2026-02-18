using Contracts.Messages;
using WorkerPayment.Data.Entities;
using WorkerPayment.Dtos.Request;
using WorkerPayment.Dtos.Response;


namespace WorkerPayment.Mappers
{
    public static class PaymentMapper
    {
        public static PagamentoDto ToPagamentoDto(PedidoCriado pedido)
        {
            return new PagamentoDto(
                pedido.Pagamento.NumeroCartao,
                pedido.Pagamento.Titular,
                pedido.Pagamento.Validade,
                pedido.Pagamento.Cvv,
                pedido.Itens.Sum(i => i.Quantidade * i.Preco)
            );
        }

        public static Payment ToPaymentEntity(PedidoCriado pedido, PaymentResponse resultado)
        {
            return new Payment
            {
                PedidoId = pedido.PedidoId,
                Valor = pedido.Itens.Sum(i => i.Quantidade * i.Preco),
                Status = resultado.Status,
                Detalhes = resultado.Detalhe,
                DataProcessamento = DateTime.Now
            };
        }

        public static PagamentoProcessado ToPagamentoProcessadoEvent(Payment payment)
        {
            return new PagamentoProcessado
            {
                PedidoId = payment.PedidoId,
                Valor = payment.Valor,
                Status = payment.Status.ToString(),
                Detalhes = payment.Detalhes ?? string.Empty,
                DataProcessamento = payment.DataProcessamento
            };
        }
    }
}
