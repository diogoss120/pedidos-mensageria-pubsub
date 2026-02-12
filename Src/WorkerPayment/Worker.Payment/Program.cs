using WorkerPayment;
using Shared.Data.Extensions;

using Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging();

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

// PubSub
var pubSubConfig = builder.Configuration.GetSection("PubSubConfig").Get<WorkerPayment.Configuration.PubSubConfig>();

if (pubSubConfig is null)
{
    throw new InvalidOperationException("PubSubConfig section is missing or invalid in appsettings.json");
}

builder.Services.AddSingleton(pubSubConfig);

builder.Services.AddSingleton<WorkerPayment.Data.Repositories.Interfaces.IPaymentRepository, WorkerPayment.Data.Repositories.PaymentRepository>();
builder.Services.AddSingleton<WorkerPayment.Services.Interfaces.IPaymentService, WorkerPayment.Services.PaymentService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
