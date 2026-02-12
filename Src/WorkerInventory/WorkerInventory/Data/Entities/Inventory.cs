using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WorkerInventory.Data.Entities;

public class Inventory
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public int Reservado { get; set; }
}
