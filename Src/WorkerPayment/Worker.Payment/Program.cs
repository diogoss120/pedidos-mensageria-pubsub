using WorkerPayment;
using Shared.Data.Extensions;

using Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging();

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddSingleton<WorkerPayment.Data.Repositories.Interfaces.IPaymentRepository, WorkerPayment.Data.Repositories.PaymentRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
