using MongoDB.Bson.Serialization.Attributes;
using Contracts.Enums;

namespace Order.Api.Data.Entities;

public class Pedido
{
    [BsonId] public Guid Id { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public StatusPedido Status { get; set; }

    public decimal Total
    {
        get => Itens.Sum(i => i.Preco);
        private set { }
    }

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
    public string Expiracao { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}
