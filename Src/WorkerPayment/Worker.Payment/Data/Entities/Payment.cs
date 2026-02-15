using Shared.Data.Entities;

namespace WorkerPayment.Data.Entities;

public class Payment : EntityBase
{
    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }
    public string? Status { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataProcessamento { get; set; }
}
