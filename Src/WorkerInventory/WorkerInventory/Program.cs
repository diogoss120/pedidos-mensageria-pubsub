using WorkerInventory;

using Messaging; // Add this line

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging(); // Add this line
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
