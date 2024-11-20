using ProjectUNIX.GameServer.Game.Entity;
using ProjectUNIX.GameServer.Game.Entity.Object;

namespace ProjectUNIX.GameServer.Game.Event
{
    internal interface IEntityEventListener
    {
        ValueTask OnEntitySpawned(SceneEntity entity);

        ValueTask OnObjectSpawned(SceneObject obj);

        ValueTask OnEntityDamage(SceneEntity entity);

        ValueTask OnEntityMove(SceneEntity entity);

        ValueTask OnEntityMotion(SceneEntity entity);
    }
}
