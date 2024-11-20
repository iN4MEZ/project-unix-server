using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Project_UNIX.AuthServer.Network.Https
{
    internal class HttpsGateway : IGateway
    {
        private readonly ILogger _logger;
        public HttpsGateway(ILogger<HttpsGateway> logger) { 
            _logger = logger;
        }

        public async Task Start()
        {
            var host = new WebHostBuilder();

            host.UseStartup<HttpsStartUp>();
           

            host.UseKestrel(options =>
            {
                options.ListenAnyIP(5001,listenOptions =>
                {
                    //listenOptions.UseHttps("C:\\Users\\in4me\\source\\repos\\Project UNIX\\Project UNIX.AuthServer\\Network\\Auth\\Certificates\\mycertificate.pfx", "Maxwell");
                });

            });

            await host.Build().RunAsync();

            await Task.CompletedTask;
        }

        public Task Stop()
        {
            _logger.LogInformation("Saving Data");
            return Task.CompletedTask;
        }
    }
}
