namespace Order.Api.Dtos
{
    public class ItemPedidoDto
    {
        public string Nome { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }
    }
}
