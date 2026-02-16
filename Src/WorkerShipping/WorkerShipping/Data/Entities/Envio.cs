using Shared.Data.Entities;

namespace WorkerShipping.Data.Entities
{
    public class Envio : EntityBase
    {
        public Guid PedidoId { get; set; }
        public string CodigoRastreio { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Detalhes { get; set; } = string.Empty;
        public DateTime DataPostagem { get; set; }


    }
}
