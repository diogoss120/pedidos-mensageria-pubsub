using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkerPayment.Data.Entities;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }
    public string? Status { get; set; }
    public DateTime DataProcessamento { get; set; }
}
