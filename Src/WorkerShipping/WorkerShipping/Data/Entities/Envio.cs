using Shared.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WorkerShipping.Data.Enums;

namespace WorkerShipping.Data.Entities
{
    public class Envio : EntityBase
    {
        public Guid PedidoId { get; set; }
        public string CodigoRastreio { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.String)]
        public ShippingStatus Status { get; set; }
        public string Detalhes { get; set; } = string.Empty;
        public DateTime DataPostagem { get; set; }


    }
}
