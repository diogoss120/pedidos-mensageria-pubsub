namespace Contracts.Messages
{
    public class PedidoDespachado
    {
        public Guid PedidoId { get; set; }
        public string CodigoRastreio { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; }
    }
}
