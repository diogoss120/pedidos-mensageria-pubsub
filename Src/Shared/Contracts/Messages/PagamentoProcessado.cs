namespace Contracts.Messages
{
    public class PagamentoProcessado
    {
        public Guid PedidoId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Detalhes { get; set; } = string.Empty;
        public DateTime DataProcessamento { get; set; }
    }
}
