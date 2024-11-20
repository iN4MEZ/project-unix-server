using Microsoft.Extensions.Logging;
using NMX.Protocal;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.Entity;
using ProjectUNIX.GameServer.Game.Entity.Object;
using ProjectUNIX.GameServer.Game.Event;
using ProjectUNIX.GameServer.Network.kcp.Session;

namespace ProjectUNIX.GameServer.Network.Manager
{
    internal class SessionEntityListener : IEntityEventListener
    {
        private readonly NetSession _session;

        private readonly ILogger _logger;

        public SessionEntityListener(ILogger<SessionEntityListener> logger,NetSession session) { 
            _logger = logger;
            _session = session;
        }

        public ValueTask OnEntityDamage(SceneEntity entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask OnEntityMotion(SceneEntity entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask OnEntityMove(SceneEntity entity)
        {
            throw new NotImplementedException();
        }

        public async ValueTask OnEntitySpawned(SceneEntity entity)
        {
            await _session.NotifyAsync(MessageId.EntityAppearNotify,new EntityAppearNotify { });
        }

        public async ValueTask OnObjectSpawned(SceneObject obj)
        {
            await _session.NotifyAsync(MessageId.SceneUpdateNotify, new ChestInteractionNotify { ChestId = obj.EntityId });
        }
    }
}
