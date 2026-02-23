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
builder.Services.AddOptions();
builder.Services.Configure<PubSubConfig>(builder.Configuration.GetSection(PubSubConfig.SectionName));

builder.Services.AddSingleton<IPaymentGateway, PaymentGateway>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
