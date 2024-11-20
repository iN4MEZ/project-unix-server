using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project_UNIX.AuthServer.Network;
using Project_UNIX.AuthServer.Network.Https;
using Project_UNIX.Common.Database;


Console.Title = "Auth Server";

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
builder.Logging.AddSimpleConsole();

builder.Services.AddSingleton<IGateway,HttpsGateway>();

builder.Services.AddHostedService<AuthServer>();


await builder.Build().RunAsync();
