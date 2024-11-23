using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project_UNIX.SDKServer.Network;
using Project_UNIX.SDKServer.Network.Https;


Console.Title = "SDK Server";

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
builder.Logging.AddSimpleConsole();

builder.Services.AddSingleton<IGateway,HttpsGateway>();

builder.Services.AddHostedService<SDKServer>();


await builder.Build().RunAsync();
