using NMX.Protocal;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Network.NetCommand.NetAttribute;
using ProjectUNIX.GameServer.Network.NetCommand.Result;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    [NetController]
    internal class PlayerHandler : HandlerBase
    {
        [NetCommand(MessageId.CscPlayerSync)]
        public ValueTask<IResult> OnPlayerSyncCs(GamePlayer gamePlayer)
        {
            CscPlayerSync cscData = Packet!.DecodeBody<CscPlayerSync>();


            ScPlayerSync rsp = new ScPlayerSync { };

            return ValueTask.FromResult(Response(MessageId.ScPlayerSync,rsp));
        }

    }
}
