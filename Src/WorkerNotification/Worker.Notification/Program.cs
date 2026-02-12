using Microsoft.Extensions.Options;
using WorkerNotification;
using WorkerNotification.Services;
using Messaging;
using WorkerNotification.Configuration;
using WorkerNotification.Services.Interfaces;
using WorkerNotification.Data.Repositories.Interfaces;
using WorkerNotification.Data.Repositories;
using Shared.Data.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<PubSubConfig>(builder.Configuration.GetSection(PubSubConfig.SectionName));

builder.Services.AddMessaging();
builder.Services.AddSingleton<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddSingleton<INotificationRepository, NotificationRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
