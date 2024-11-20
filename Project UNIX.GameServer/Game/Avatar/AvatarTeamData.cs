using NMX.Protocal;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Game.Player.TeamManager;
using ProjectUNIX.GameServer.Utils.DataCollection.Excels;
namespace ProjectUNIX.GameServer.Game.Avatar
{
    internal class AvatarTeamData
    {
        public TeamMember[] Members;

        private readonly GamePlayer _player;

        public int MaxTeamIndex { get; set; } = 3;

        public AvatarTeamData(GamePlayer gamePlayer)
        {
            Members = new TeamMember[MaxTeamIndex];
            _player = gamePlayer;
        }

        public void Join(uint avatarId, uint slot)
        {

            AvatarExcel data = _player.excelData.GetTable(ExcelType.Avatar).GetItemById<AvatarExcel>(avatarId);
            if (data == null || slot > 4) return;

            Members[slot] = new(data);

        }

        public void ClearMember()
        {
            Array.Clear(Members);
        }
        public TeamInfo TeamInfo
        {
            get
            {
                TeamInfo team = new TeamInfo
                {
                    TeamIndex = 0,
                };
                foreach (TeamMember member in Members)
                {
                    if (member != null)
                    {
                        team.AvatarList.Add(new AvatarData
                        {
                            Id = member.Id,
                            SInfo = new AvatarBinSkillInfo() { BaseAtk = member.baseAtk, BaseDef = 0, BaseHp = member.baseHp }
                        });

                    }
                }
                return team;

            }
        }

    }
}
