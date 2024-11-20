using Google.Protobuf;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.GameManager;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Network.Manager;

namespace ProjectUNIX.GameServer.Game.World
{
    internal class PlayerWorld
    {
        public long WorldId;

        private readonly ILogger _logger;

        private readonly GamePlayer _host;

        private List<GamePlayer> _players;

        private int _worldLevel;

        private uint worldCounter;

        private readonly PlayerWorldManager _worldManager;


        public bool isMultiplayer {  get; private set; }

        public PlayerWorld(ILogger<PlayerWorld> logger,GamePlayer host, PlayerWorldManager worldManager)
        {
            _logger = logger;

            _host = host;

            _players = new();

            _worldManager = worldManager;

        }

        public void AddPlayer(GamePlayer target)
        {
            _players.Add(target);
        }

        public void RemovePlayer( GamePlayer target)
        {
            _players.Remove(target);
        }

        public GamePlayer GetHost()
        {
            return _host;
        }
        public async ValueTask NotifyAll<TNotify>(MessageId cmdType, TNotify message) where TNotify : IMessage<TNotify>
        {
            foreach (GamePlayer player in _players)
            {
                await player.GetSession().NotifyAsync(cmdType, message);
            }
        }

        public async ValueTask NotifyAllExceptHost<TNotify>(MessageId cmdType, TNotify message) where TNotify : IMessage<TNotify>
        {
            foreach (GamePlayer player in _players)
            {
               if(player != _host)
                {
                    await player.GetSession().NotifyAsync(cmdType, message);
                }
            }
        }

        public void CreateWorld()
        {
            Random rand = new Random();

            AddPlayer(_host);

            uint convId = Interlocked.Increment(ref worldCounter);
            uint token = (uint)rand.Next();

            long convId64 = convId << 32 | token;

            WorldId = convId64;

            _worldManager.Add(this);
        }
    }
}
