using WorkerPayment;
using Shared.Data.Extensions;

using Messaging;
using WorkerPayment.Data.Repositories.Interfaces;
using WorkerPayment.Services.Interfaces;
using WorkerPayment.Data.Repositories;
using WorkerPayment.Services;
using WorkerPayment.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging();

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

// PubSub
var pubSubConfig = builder.Configuration.GetSection("PubSubConfig").Get<PubSubConfig>();

if (pubSubConfig is null)
{
    throw new InvalidOperationException("PubSubConfig section is missing or invalid in appsettings.json");
}

builder.Services.AddSingleton(pubSubConfig);

builder.Services.AddSingleton<IPaymentGateway, PaymentGateway>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
