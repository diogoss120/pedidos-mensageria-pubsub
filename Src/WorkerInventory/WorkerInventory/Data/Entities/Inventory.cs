using Shared.Data.Entities;

namespace WorkerInventory.Data.Entities;

public class Inventory : EntityBase
{

    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public int Reservado { get; set; }
}
