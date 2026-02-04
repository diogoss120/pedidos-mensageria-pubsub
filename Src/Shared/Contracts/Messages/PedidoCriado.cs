namespace Contracts.Messages
{
    public class PedidoCriado
    {
        public Guid CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid PedidoId { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }

        public Cliente Cliente { get; set; } = new();

        public List<ItemPedido> Itens { get; set; } = new();

        public Pagamento Pagamento { get; set; } = new();
    }

    public class Cliente
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class ItemPedido
    {
        public string Nome { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }
    }

    public class Pagamento
    {
        public string NumeroCartao { get; set; } = string.Empty;

        public string Titular { get; set; } = string.Empty;

        public string Validade { get; set; } = string.Empty;

        public string Cvv { get; set; } = string.Empty;
    }
}
