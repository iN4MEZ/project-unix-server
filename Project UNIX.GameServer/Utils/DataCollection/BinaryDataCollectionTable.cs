using Microsoft.Extensions.Logging;
using ProjectUNIX.GameServer.Utils.DataCollection.Assets;
using ProjectUNIX.GameServer.Utils.DataCollection.Excels;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using ProjectUNIX.GameServer.Utils.DataCollection.Binary;
using ProjectUNIX.GameServer.Utils.DataCollection.Binary.AttributeClass;

namespace ProjectUNIX.GameServer.Utils.DataCollection
{
    internal class BinaryDataCollectionTable
    {
        private readonly ImmutableDictionary<BinType, BinaryTable> _tables;

        public BinaryDataCollectionTable(IAssets assets, ILogger<BinaryDataCollectionTable> logger)
        {
            _tables = LoadTables(assets);

            logger.LogInformation("Bin Loaded: " + _tables.Count);
        }

        private static ImmutableDictionary<BinType, BinaryTable> LoadTables(IAssets assetProvider)
        {
            ImmutableDictionary<BinType, BinaryTable>.Builder tables = ImmutableDictionary.CreateBuilder<BinType, BinaryTable>();

            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                                      .Where(type => type.GetCustomAttribute<BinaryAttribute>() != null);

            foreach (Type type in types)
            {
                BinaryAttribute attribute = type.GetCustomAttribute<BinaryAttribute>()!;

                JsonDocument tableJson = assetProvider.GetBinTableJson(attribute.AssetName);

                tables.Add(attribute.Type, new BinaryTable(tableJson, type));
            }

            return tables.ToImmutable();
        }

        public BinaryTable GetTable(BinType type)
        {
            return _tables[type];
        }
    }
}
