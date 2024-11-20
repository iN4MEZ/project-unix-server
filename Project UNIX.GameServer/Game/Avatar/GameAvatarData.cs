using ProjectUNIX.GameServer.Utils.DataCollection.Excels;

namespace ProjectUNIX.GameServer.Game.Avatar
{
    internal class GameAvatarData
    {
        public string AvatarName;
        public uint Id;
        public float baseAtk;
        public int baseHp;

        public GameAvatarData(AvatarExcel avatarExcel)
        {
            AvatarName = "s";
            Id = avatarExcel.Id;
            baseAtk = avatarExcel.baseAtk;
            baseHp = avatarExcel.baseHp;
        }
    }
}
