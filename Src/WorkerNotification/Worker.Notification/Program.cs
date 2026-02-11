using Microsoft.Extensions.Options;
using WorkerNotification;
using WorkerNotification.Services;
using Messaging;
using WorkerNotification.Configuration;
using WorkerNotification.Services.Interfaces;
using WorkerNotification.Data.Settings;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using WorkerNotification.Data.Repositories.Interfaces;
using WorkerNotification.Data.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<PubSubConfig>(builder.Configuration.GetSection(PubSubConfig.SectionName));

builder.Services.AddMessaging();
builder.Services.AddSingleton<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Configure Guid to be stored as Standard definition in MongoDB
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.AddSingleton<INotificationRepository, NotificationRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
