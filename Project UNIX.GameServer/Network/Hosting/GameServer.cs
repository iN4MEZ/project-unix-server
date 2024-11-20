using Microsoft.Extensions.Hosting;
using ProjectUNIX.GameServer.Utils.DataCollection;
using ProjectUNIX.GameServer.Utils.DataCollection.Quests;

namespace ProjectUNIX.GameServer.Network.Hosting
{
    internal class GameServer : IHostedService
    {
        private readonly IGateway _gateway;

        public GameServer(IGateway gateway, ExcelDataCollectionTable excelTables, QuestDataCollectionTable questTables, BinaryDataCollectionTable binaryDataCollectionTable)
        {

            _ = excelTables;
            _ = questTables;
            _ = binaryDataCollectionTable;

            _gateway = gateway;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _gateway.Run();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _gateway.Stop();
        }
    }
}
