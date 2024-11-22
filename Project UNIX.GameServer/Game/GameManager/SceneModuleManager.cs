using Microsoft.Extensions.Logging;
using NMX.Protocal;
using Org.BouncyCastle.Asn1.X509;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.Entity;
using ProjectUNIX.GameServer.Game.Entity.Object;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Game.Scene;
using ProjectUNIX.GameServer.Game.World;
using ProjectUNIX.GameServer.Network.kcp.Session;

namespace ProjectUNIX.GameServer.Game.GameManager
{
    internal class SceneModuleManager
    {

        private readonly GamePlayer _player;
        private readonly NetSession _session;
        private readonly EntityModuleManager _entityModuleManager;

        private readonly ILogger _logger;

        private uint _sceneId;

        private SceneEnterState _currentEnterState;

        private List<AvatarEntity> _avatars;

        private readonly PlayerWorld _world;

        private readonly PlayerWorldManager _worldManager;

        public SceneModuleManager(GamePlayer player,NetSession session,EntityModuleManager entityModuleManager, ILogger<SceneModuleManager> logger,PlayerWorld world,PlayerWorldManager playerWorldManager)
        {

            _player = player;

            _session = session;
            _entityModuleManager = entityModuleManager;
            _logger = logger;
            _worldManager = playerWorldManager;

            _world = world;
            _avatars = new();
            _world.CreateWorld();
        }


        public void OnClientChangedLoadingState(SceneEnterState newLoadingState)
        {
            _currentEnterState = newLoadingState;

            switch (_currentEnterState)
            {
                case SceneEnterState.Request:

                    break;
                case SceneEnterState.Ready:
                    _logger.LogInformation("ID {id} Ready!", _player.Id);
                    break;
                case SceneEnterState.Pre:
                    OnClientLoadingPreState();
                    break;
                case SceneEnterState.Post:

                    break;
                case SceneEnterState.Finish:
                    _logger.LogInformation("ID {id} Finish Load!", _player.Id);
                    break;
            }
        }

        private void OnClientLoadingPreState()
        {
            _player.SetPosition(new Vector { X = 0, Y = 99, Z = 0});
        }

        public async ValueTask EnterSceneAsync(uint sceneId,EnterReason reason)
        {
            _sceneId = sceneId;

            SceneInfo sceneInfo = CreateSceneInfo(sceneId);

            await _session.NotifyAsync(MessageId.PlayerEnterSceneNotify,new PlayerEnterSceneNotify
            { HostId = _player.Id,Reason = reason, SceneInfo = sceneInfo });
        }
        public async ValueTask ChangeSceneAsync(uint sceneId, EnterReason reason)
        {
            _sceneId = sceneId;

            SceneInfo sceneInfo = new SceneInfo { SceneId = _sceneId, InitPos = new Vector { X = 0, Y = 1, Z = 0 } };

            await _session.NotifyAsync(MessageId.PlayerChangeSceneNotify, new PlayerEnterSceneNotify
            { HostId = _player.Id, Reason = reason, SceneInfo = sceneInfo });
        }

        public async ValueTask ChangeSceneNotifyAllAsync(PlayerWorld world,uint sceneId, EnterReason reason)
        {
            _sceneId = sceneId;

            SceneInfo sceneInfo = new SceneInfo { SceneId = _sceneId, InitPos = new Vector { X = 0, Y = 1, Z = 0 } };

            await world.NotifyAll(MessageId.PlayerChangeSceneNotify, new PlayerEnterSceneNotify
            { HostId = _player.Id, Reason = reason, SceneInfo = sceneInfo });

            await world.NotifyAll(MessageId.EntityAppearNotify, new EntityAppearNotify
            { });
        }

        public async Task EnterMultiplayer(long requestTo)
        {
            PlayerWorld target = _worldManager.GetWorldById(requestTo);

            if (target == null) { return; }

            bool isSelf = (target.WorldId == _world.WorldId) ? true : false;

            if (isSelf) { return; }

            _logger.LogInformation("World: {0} to {1}", _world.WorldId, target.WorldId);

            target.AddPlayer(_player);

            await ChangeSceneNotifyAllAsync(target,2, EnterReason.RequestToOther);

        }

        public SceneInfo CreateSceneInfo(uint instanceId)
        {
            EntityInfo entityInfo = new EntityInfo { Id = 200001, EType = EntityType.Monster };

            SceneInfo si = new SceneInfo { SceneId = instanceId, InitPos = new Vector { X = 0, Y = 0, Z = 0 }, EntityList = { entityInfo } };

            return si;
        }

        public async ValueTask SpawnChestNotify(SceneObject obj)
        {
            await _entityModuleManager.OnObjectSpawned(obj);
        }

        public async ValueTask EntitySpawnNotify(SceneObject e)
        {
            await _entityModuleManager.OnEntitySpawned(e);
        }
        public async ValueTask ChestInteractionNotify(uint chestId)
        {
            await _session.NotifyAsync(MessageId.SceneChestUpdateNotify, new ChestInteractionNotify { ChestId = chestId });
        }

        public async ValueTask OnEntityMoving(uint entityId,Vector pos)
        {
            await _session.NotifyAsync(MessageId.SceneEntityMoveUpdateNotify, new EntityMoving { EntityId = entityId,MoveInfo = new MovingInfo { Pos = pos} });
        }

        public async ValueTask OnEntityMovingWorld(uint entityId, Vector pos)
        {
            await _world.NotifyAll(MessageId.SceneEntityMoveUpdateNotify, new SceneEntityMovingUpdateNotify { EInfo = new EntityInfo { Id = entityId }, NewPos = pos});
        }


    }
}
