using WorkerInventory.Data.Entities;

namespace WorkerInventory.Data.Repositories.Interfaces;

public interface IInventoryRepository
{
    Task CreateAsync(Inventory inventory);
}
