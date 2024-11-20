using NMX.Protocal;

namespace ProjectUNIX.GameServer.Utils.Json
{
    internal class PlayerInfoData
    {
        public PlayerProp? PlayerProp { get; set; }

        public string? PlayerGameObjectPath { get; set; }

        public string? PlayerCamGameObjectPath { get; set; }
    }
}
