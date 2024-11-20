using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project_UNIX.Common.Database;

namespace Project_UNIX.AuthServer.Network
{
    internal class AuthServer : IHostedService
    {
        private readonly IGateway _gateway;

        public AuthServer(IGateway gateway) {
            _gateway = gateway;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _gateway.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _gateway.Stop();
        }
    }
}
