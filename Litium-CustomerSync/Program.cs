using Litium.Accelerator.Services;
using Microsoft.Extensions.Http;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient<CustomerSyncService>();

var host = builder.Build();
host.Run();
