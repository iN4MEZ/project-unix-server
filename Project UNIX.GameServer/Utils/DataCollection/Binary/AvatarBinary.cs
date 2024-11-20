using ProjectUNIX.GameServer.Utils.DataCollection.Binary.AttributeClass;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Binary
{
    [Binary(BinType.Avatar, "AvatarBinaryDataConfigTable.json")]
    internal class AvatarBinary : BinaryItem
    {
        public override uint BinId => BinId;

        [JsonPropertyName("AvatarId")]
        public string Id { get; set; }  
    }
}
