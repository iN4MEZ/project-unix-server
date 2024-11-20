using Microsoft.Extensions.Logging;
using ProjectUNIX.GameServer.Utils.DataCollection;

namespace ProjectUNIX.GameServer.Utils
{
    internal class DataHelper
    {
        public ILogger _logger { get; private set; }
        public ExcelDataCollectionTable _table { get; private set; }
    }
}
