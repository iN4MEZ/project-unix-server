using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using ProjectUNIX.GameServer.Utils.DataCollection.Assets;
using ProjectUNIX.GameServer.Utils.DataCollection.Excels.AttributeClass;
using ProjectUNIX.GameServer.Utils.DataCollection.Excels;

namespace ProjectUNIX.GameServer.Utils.DataCollection
{
    internal class ExcelDataCollectionTable
    {
        private readonly ImmutableDictionary<ExcelType, ExcelTable> _tables;

        public ExcelDataCollectionTable(IAssets assets, ILogger<ExcelDataCollectionTable> logger)
        {
            _tables = LoadTables(assets);

            logger.LogInformation("Excel Loaded: " + _tables.Count);
        }

        private static ImmutableDictionary<ExcelType, ExcelTable> LoadTables(IAssets assetProvider)
        {
            ImmutableDictionary<ExcelType, ExcelTable>.Builder tables = ImmutableDictionary.CreateBuilder<ExcelType, ExcelTable>();

            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                                      .Where(type => type.GetCustomAttribute<ExcelAttribute>() != null);

            foreach (Type type in types)
            {
                ExcelAttribute attribute = type.GetCustomAttribute<ExcelAttribute>()!;

                JsonDocument tableJson = assetProvider.GetExcelTableJson(attribute.AssetName);

                tables.Add(attribute.Type, new ExcelTable(tableJson, type));
            }

            return tables.ToImmutable();
        }

        public ExcelTable GetTable(ExcelType type)
        {
            return _tables[type];
        }
    }
}
