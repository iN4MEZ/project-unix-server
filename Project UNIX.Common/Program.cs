using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Project_UNIX.Common;
using Project_UNIX.Common.Database;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Logging.AddSimpleConsole();

builder.Services.AddSingleton<DatabaseHandler>();

builder.Services.AddHostedService<CommonServices>();

await builder.Build().RunAsync();
