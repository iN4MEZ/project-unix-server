using ProjectUNIX.GameServer.Game.Entity;
using ProjectUNIX.GameServer.Game.Entity.Object;
using ProjectUNIX.GameServer.Game.Event;

namespace ProjectUNIX.GameServer.Game.GameManager
{
    internal class EntityModuleManager
    {
        private readonly IEntityEventListener _listener;
        private List<SceneEntity> entitys;
        private List<SceneObject> objects;


        public EntityModuleManager(IEntityEventListener listener) {
            _listener = listener;
            entitys = new();
            objects = new();
            
        }

        public async ValueTask OnEntitySpawned(SceneEntity sceneEntity)
        {
            entitys.Add(sceneEntity);
            await _listener.OnEntitySpawned(sceneEntity);
        }
        public async ValueTask OnObjectSpawned(SceneObject sceneEntity)
        {
            objects.Add(sceneEntity);
            await _listener.OnObjectSpawned(sceneEntity);
        }
    }
}
