using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Text.Json;
using ProjectUNIX.GameServer.Utils.DataCollection.Assets;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Quests
{
    internal class QuestDataCollectionTable
    {
        private readonly ImmutableDictionary<int, QuestData> _quests;

        private readonly ILogger _logger;

        public QuestDataCollectionTable(ILogger<QuestDataCollectionTable> logger, IAssets assets)
        {
            _logger = logger;

            _quests = LoadQuestData(assets);

            _logger.LogInformation("Loaded {quest} Quest", _quests.Count);
        }

        ImmutableDictionary<int, QuestData> LoadQuestData(IAssets assets)
        {
            string[] allfile = assets.GetAllQuestFile();

            ImmutableDictionary<int, QuestData>.Builder tables = ImmutableDictionary.CreateBuilder<int, QuestData>();

            foreach (string file in allfile)
            {
                try
                {
                    string jsonContent = File.ReadAllText(file);

                    QuestData jsonObj = JsonSerializer.Deserialize<QuestData>(jsonContent)!;

                    tables.Add(jsonObj.Id, jsonObj);

                }
                catch (Exception ex)
                {
                    _logger.LogError("Error: {ex}", ex);
                }

            }

            return tables.ToImmutable();
        }

        public QuestData GetQuestDataById(int id)
        {
            return _quests[id];
        }

    }
}
