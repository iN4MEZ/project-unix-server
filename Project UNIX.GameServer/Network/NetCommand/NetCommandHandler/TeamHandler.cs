using NMX.Protocal;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Network.NetCommand.NetAttribute;
using ProjectUNIX.GameServer.Network.NetCommand.Result;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    [NetController]
    internal class TeamHandler : HandlerBase
    {
        private uint[] teamTempoId = { 10001, 10005 };

        [NetCommand(MessageId.GetTeamDataReq)]
        public ValueTask<IResult> GetTeamDataReq(NetSession session, GamePlayer player)
        {
            for (uint i = 0; i < teamTempoId.Length; i++)
            {
                player.TeamData.Join(teamTempoId[i], i);
            }

            return ValueTask.FromResult(Response(MessageId.GetTeamDataRsp, new GetTeamDataRsp
            {
                TeamInfo = player.TeamData.TeamInfo
            }));
        }

        [NetCommand(MessageId.PlayerSwitchAvatarReq)]
        public ValueTask<IResult> SwitchAvatarReq(NetSession session, GamePlayer player)
        {
            PlayerSwitchActiveReq req = Packet!.DecodeBody<PlayerSwitchActiveReq>();

            PlayerSwitchActiveRsp rsp = new PlayerSwitchActiveRsp { Index = req.Index };

            return ValueTask.FromResult(Response(MessageId.PlayerSwitchAvatarRsp, rsp));
        }

        [NetCommand(MessageId.GetAvatarDataReq)]
        public ValueTask<IResult> GetAvatarData()
        {
            return ValueTask.FromResult(Response(MessageId.GetAvatarDataRsp, new GetAvatarDataRsp
            {
                AvatarList = { new AvatarData { Id = 10001 } }
            }));

        }
    }
}
