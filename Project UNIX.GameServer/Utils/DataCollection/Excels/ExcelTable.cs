using System.Collections.Immutable;
using System.Text.Json;


namespace ProjectUNIX.GameServer.Utils.DataCollection.Excels
{
    internal class ExcelTable
    {
        private readonly ImmutableArray<ExcelItem> _items;

        public ExcelTable(JsonDocument jsonDocument, Type type)
        {
            _items = LoadData(jsonDocument, type);
        }

        private ImmutableArray<ExcelItem> LoadData(JsonDocument jsonDocument, Type type)
        {
            ImmutableArray<ExcelItem>.Builder items = ImmutableArray.CreateBuilder<ExcelItem>();

            foreach (JsonElement element in jsonDocument.RootElement.EnumerateArray())
            {
                if (element.ValueKind != JsonValueKind.Object)
                    throw new ArgumentException($"ExcelTable::LoadData - expected an object, got {element.ValueKind}");

                ExcelItem deserialized = (element.Deserialize(type) as ExcelItem)!;


                items.Add(deserialized);
            }

            return items.ToImmutable();
        }
        public TExcel GetItemAt<TExcel>(int index) where TExcel : ExcelItem
        {
            return (_items[index] as TExcel)!;
        }
        public TExcel GetItemById<TExcel>(uint id) where TExcel : ExcelItem
        {
            try
            {
                foreach (var item in _items)
                {
                    if (item.ExcelId == id)
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
