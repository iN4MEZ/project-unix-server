using Microsoft.Extensions.Logging;
using NMX.Protocal;
using ProjectUNIX.GameServer.Game.Avatar;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Utils.DataCollection;
using ProjectUNIX.GameServer.Utils.DataCollection.Excels;

namespace ProjectUNIX.GameServer.Game.Player
{
    internal class GamePlayer : IDisposable
    {

        private readonly NetSession _session;

        public string PlayerName { get; private set; }

        public List<GameAvatarData> Avatars { get; private set; }

        public Vector Pos {  get; private set; }

        public Vector PrvPos { get; private set; }

        public uint Id { get; private set; }

        public readonly AvatarTeamData TeamData;

        public readonly ExcelDataCollectionTable excelData;

        private readonly BinaryDataCollectionTable _binaryData;

        private readonly ILogger _logger;


        public GamePlayer(NetSession session, ExcelDataCollectionTable excelDataCollectionTable, ILogger<GamePlayer> logger, BinaryDataCollectionTable binaryData)
        {
            excelData = excelDataCollectionTable;
            _session = session;
            TeamData = new(this);
            Avatars = new();
            PlayerName = "";
            Pos = new Vector();
            PrvPos = new Vector();

            _logger = logger;
            _binaryData = binaryData;
        }

        public void InitializePlayerDefault(uint id)
        {
            PlayerName = "TestPlayer";

            Id = id;

            InitAvatar();
        }

        public void InitAvatar()
        {
            try
            {
                for (uint i = 10000; i < 20000; i++)
                {
                    AvatarExcel data = excelData.GetTable(ExcelType.Avatar).GetItemById<AvatarExcel>(i);
                    if (data != null)
                    {
                        Avatars.Add(new(data));

                        //_logger.LogInformation(data.Id.ToString());

                        continue;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public void SetPosition(Vector position)
        {
            Pos = position;
        }

        public void Dispose()
        {
            TeamData.ClearMember();
        }

        public NetSession GetSession()
        {
            return _session;
        }
    }
}
