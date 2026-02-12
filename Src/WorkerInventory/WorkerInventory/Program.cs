using WorkerInventory;

using Messaging; // Add this line

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging(); // Add this line

// MongoDB
builder.Services.Configure<WorkerInventory.Data.Settings.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDB.Driver.IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<WorkerInventory.Data.Settings.MongoDbSettings>>().Value;
    return new MongoDB.Driver.MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<MongoDB.Driver.IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<WorkerInventory.Data.Settings.MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<MongoDB.Driver.IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Configure Guid to be stored as Standard definition in MongoDB
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

builder.Services.AddSingleton<WorkerInventory.Data.Repositories.Interfaces.IInventoryRepository, WorkerInventory.Data.Repositories.InventoryRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
