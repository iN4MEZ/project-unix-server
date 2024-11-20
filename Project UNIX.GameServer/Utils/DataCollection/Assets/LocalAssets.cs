using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Assets
{
    internal sealed class LocalAssets : IAssets
    {
        private const string ExcelDirectory = "data/";
        private const string BinDirectory = "data/bin/";
        private const string AvatarConfigDirectory = "assets/binout/avatar/";

        private const string QuestFileDirectory = "data/quest";

        public IEnumerable<string> EnumerateAvatarConfigFiles()
        {
            return Directory.GetFiles(AvatarConfigDirectory);
        }

        public string[] GetAllQuestFile()
        {
            return Directory.GetFiles(QuestFileDirectory);
        }

        public JsonDocument GetFileAsJsonDocument(string fullPath)
        {
            using FileStream fileStream = new(fullPath, FileMode.Open, FileAccess.Read);
            return JsonDocument.Parse(fileStream);
        }

        public JsonDocument GetExcelTableJson(string assetName)
        {
            string filePath = string.Concat(ExcelDirectory, assetName);
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);

            return JsonDocument.Parse(fileStream);
        }

        public JsonDocument GetBinTableJson(string assetName)
        {
            string filePath = string.Concat(BinDirectory, assetName);
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);

            return JsonDocument.Parse(fileStream);
        }
    }
}
