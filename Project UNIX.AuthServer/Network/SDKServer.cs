using Microsoft.Extensions.Hosting;

namespace Project_UNIX.SDKServer.Network
{
    internal class SDKServer : IHostedService
    {
        private readonly IGateway _gateway;

        public SDKServer(IGateway gateway) {
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
