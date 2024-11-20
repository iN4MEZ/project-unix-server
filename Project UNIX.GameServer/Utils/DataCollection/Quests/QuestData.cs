using System.Text.Json.Serialization;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Quests
{
    internal class QuestData : QuestDataItem
    {
        public override int QuestId => Id;

        [JsonPropertyName("Id")]
        public int Id { get; set; }
    }
}
