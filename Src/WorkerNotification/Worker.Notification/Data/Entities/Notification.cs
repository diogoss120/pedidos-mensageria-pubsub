using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkerNotification.Data.Entities;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    public string? ClienteNome { get; set; }
    public string? ConteudoEmail { get; set; }
    public DateTime DataEnvio { get; set; }
}
