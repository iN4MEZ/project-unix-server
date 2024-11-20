using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Network.Manager;
using System.Collections.Concurrent;

namespace ProjectUNIX.GameServer.Game.World
{
    internal class PlayerWorldManager
    {

        private readonly ConcurrentDictionary<long, PlayerWorld> _worlds;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public PlayerWorldManager(ILogger<NetSessionManager> logger, IServiceScopeFactory scopeFactory)
        {
            _worlds = new();

            this.scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Add(PlayerWorld world)
        {
            _worlds[world.WorldId] = world;
            _logger.LogInformation("World Added! Id:{0} host:{1}",world.WorldId,world.GetHost().Id);
        }

        public PlayerWorld GetWorldById(long id) => _worlds.GetValueOrDefault(id);

    }
}
