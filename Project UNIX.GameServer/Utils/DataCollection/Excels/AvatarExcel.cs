using ProjectUNIX.GameServer.Utils.DataCollection.Excels.AttributeClass;
using System.Text.Json.Serialization;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Excels
{
    [Excel(ExcelType.Avatar, "AvatarExcelConfigTable.json")]
    internal class AvatarExcel : ExcelItem
    {
        public override uint ExcelId => Id;

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("baseAtk")]
        public int baseAtk { get; set; }

        [JsonPropertyName("baseHp")]
        public int baseHp { get; set; }
    }
}
