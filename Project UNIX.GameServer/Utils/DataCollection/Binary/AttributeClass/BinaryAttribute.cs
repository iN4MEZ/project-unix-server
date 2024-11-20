using ProjectUNIX.GameServer.Utils.DataCollection.Excels;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Binary.AttributeClass
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class BinaryAttribute : Attribute
    {
        public BinType Type { get; }
        public string AssetName { get; }

        public BinaryAttribute(BinType type, string assetName)
        {
            Type = type;
            AssetName = assetName;
        }
    }
}
