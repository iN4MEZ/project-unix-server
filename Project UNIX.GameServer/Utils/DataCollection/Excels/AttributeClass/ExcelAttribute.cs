using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Utils.DataCollection.Excels.AttributeClass
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class ExcelAttribute : Attribute
    {
        public ExcelType Type { get; }
        public string AssetName { get; }

        public ExcelAttribute(ExcelType type, string assetName)
        {
            Type = type;
            AssetName = assetName;
        }
    }
}
