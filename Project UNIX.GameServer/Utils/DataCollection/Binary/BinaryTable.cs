using ProjectUNIX.GameServer.Utils.DataCollection.Excels;
using System.Collections.Immutable;
using System.Text.Json;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Binary
{
    internal class BinaryTable
    {
        private readonly ImmutableArray<BinaryItem> _items;

        public BinaryTable(JsonDocument jsonDocument, Type type)
        {
            _items = LoadData(jsonDocument, type);
        }

        private ImmutableArray<BinaryItem> LoadData(JsonDocument jsonDocument, Type type)
        {
            ImmutableArray<BinaryItem>.Builder items = ImmutableArray.CreateBuilder<BinaryItem>();

            foreach (JsonElement element in jsonDocument.RootElement.EnumerateArray())
            {
                if (element.ValueKind != JsonValueKind.Object)
                    throw new ArgumentException($"ExcelTable::LoadData - expected an object, got {element.ValueKind}");

                BinaryItem deserialized = (element.Deserialize(type) as BinaryItem)!;


                items.Add(deserialized);
            }

            return items.ToImmutable();
        }
        public TExcel GetItemAt<TExcel>(int index) where TExcel : BinaryItem
        {
            return (_items[index] as TExcel)!;
        }
        public TExcel GetItemById<TExcel>(uint id) where TExcel : BinaryItem
        {
            try
            {
                foreach (var item in _items)
                {
                    if (item.BinId == id)
                    {
                        return (item as TExcel)!;
                    }
                }
                return null!;
            }
            catch
            {
                throw new ArgumentException();
            }
        }
    }
}
