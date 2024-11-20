using Microsoft.Extensions.Hosting;
using Project_UNIX.Common.Database;

namespace Project_UNIX.Common
{
    internal class CommonServices : IHostedService
    {
        private readonly DatabaseHandler _databaseHandler;

        public CommonServices(DatabaseHandler databaseHandler)
        {
            _databaseHandler = databaseHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
