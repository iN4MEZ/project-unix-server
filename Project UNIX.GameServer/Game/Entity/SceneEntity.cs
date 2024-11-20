using NMX.Protocal;

namespace ProjectUNIX.GameServer.Game.Entity
{
    internal abstract class SceneEntity
    {
        public uint EntityId { get; }

        public string ModelPath { get; }

        public Vector Pos { get; }

        public SceneEntity(uint entityId)
        {
            EntityId = entityId;
            ModelPath = "";
            Pos = new Vector();
        }

        public virtual void SetPosition(Vector newPos)
        {
            Pos.X = newPos.X;
            Pos.Y = newPos.Y;
            Pos.Z = newPos.Z;
        }
    }
}
