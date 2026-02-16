using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Data.Entities;
using WorkerPayment.Data.Enums;

namespace WorkerPayment.Data.Entities;

public class Payment : EntityBase
{
    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }
    [BsonRepresentation(BsonType.String)]
    public PaymentStatus Status { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataProcessamento { get; set; }
}
