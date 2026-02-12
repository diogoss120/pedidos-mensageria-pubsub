using WorkerPayment;

using Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging();

// MongoDB
builder.Services.Configure<WorkerPayment.Data.Settings.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDB.Driver.IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<WorkerPayment.Data.Settings.MongoDbSettings>>().Value;
    return new MongoDB.Driver.MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<MongoDB.Driver.IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<WorkerPayment.Data.Settings.MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<MongoDB.Driver.IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Configure Guid to be stored as Standard definition in MongoDB
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

builder.Services.AddSingleton<WorkerPayment.Data.Repositories.Interfaces.IPaymentRepository, WorkerPayment.Data.Repositories.PaymentRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
