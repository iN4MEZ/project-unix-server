using ProjectUNIX.GameServer.Utils.DataCollection.Excels.AttributeClass;
using System.Text.Json.Serialization;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Excels
{
    [Excel(ExcelType.Monster, "MonsterExcelConfigTable.json")]
    internal class MonsterExcel : ExcelItem
    {
        public override uint ExcelId => Id;

        [JsonPropertyName("id")]
        public uint Id { get; set; }
    }
}
