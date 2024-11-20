using ProjectUNIX.GameServer.Utils.DataCollection.Excels;

namespace ProjectUNIX.GameServer.Game.Player.TeamManager
{
    internal class TeamMember
    {
        public readonly uint Id;

        public readonly int baseAtk;

        public readonly int baseHp;



        public TeamMember(AvatarExcel excels)
        {
            Id = excels.Id;
            baseAtk = excels.baseAtk;
            baseHp = excels.baseHp;
        }
    }
}
