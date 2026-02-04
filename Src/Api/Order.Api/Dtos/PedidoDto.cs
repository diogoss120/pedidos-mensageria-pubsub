namespace Order.Api.Dtos
{
    public class PedidoDto
    {
        public ClienteDto Cliente { get; set; } = new();

        public List<ItemPedidoDto> Itens { get; set; } = new();

        public PagamentoDto Pagamento { get; set; } = new();
    }
}
