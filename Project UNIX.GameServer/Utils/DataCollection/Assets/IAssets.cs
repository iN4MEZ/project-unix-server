using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Assets
{
    public interface IAssets
    {
        JsonDocument GetExcelTableJson(string assetName);

        JsonDocument GetBinTableJson(string assetName);

        IEnumerable<string> EnumerateAvatarConfigFiles();
        JsonDocument GetFileAsJsonDocument(string fullPath);

        string[] GetAllQuestFile();


    }
}
