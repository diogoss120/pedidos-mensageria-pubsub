using MongoDB.Driver;
using WorkerInventory.Data.Entities;
using WorkerInventory.Data.Repositories.Interfaces;

namespace WorkerInventory.Data.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly IMongoCollection<Inventory> _inventoryCollection;

    public InventoryRepository(IMongoDatabase database)
    {
        _inventoryCollection = database.GetCollection<Inventory>("Estoque");
    }

    public async Task CreateAsync(Inventory inventory)
    {
        await _inventoryCollection.InsertOneAsync(inventory);
    }
}
