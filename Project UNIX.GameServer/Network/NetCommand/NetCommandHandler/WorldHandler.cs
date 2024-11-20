using NMX.Protocal;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.GameManager;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Game.World;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Network.NetCommand.NetAttribute;
using ProjectUNIX.GameServer.Network.NetCommand.Result;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    [NetController]
    internal class WorldHandler : HandlerBase
    {
        [NetCommand(MessageId.PlayerMovingReq)]
        public async ValueTask<IResult> OnPlayerMoving(NetSession session, GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            EntityMoving req = Packet!.DecodeBody<EntityMoving>();

            //Console.WriteLine(req.EntityId + " Move To:" + req.MoveInfo.Pos);

            await sceneModuleManager.OnEntityMovingWorld(5,req.MoveInfo.Pos);

            ScPlayerSync rsp = new ScPlayerSync { };

            return (Response(MessageId.ScPlayerSync, rsp));
        }

        [NetCommand(MessageId.PlayerEnterMpReq)]
        public async ValueTask<IResult> PlayerEnterMpReq(NetSession session, GamePlayer player, SceneModuleManager sceneModuleManager,PlayerWorld world)
        {
            PlayerEnterMpReq req = Packet!.DecodeBody<PlayerEnterMpReq>();

            await sceneModuleManager.EnterMultiplayer(req.ToHost);

            PlayerEnterMpRsp rsp = new PlayerEnterMpRsp { IsAgree = true };

            return (Response(MessageId.PlayerEnterMpRsp, rsp));
        }

    }
}
