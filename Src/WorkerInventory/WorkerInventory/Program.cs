using WorkerInventory;
using Shared.Data.Extensions;

using Messaging; // Add this line

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging(); // Add this line

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddSingleton<WorkerInventory.Data.Repositories.Interfaces.IInventoryRepository, WorkerInventory.Data.Repositories.InventoryRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
