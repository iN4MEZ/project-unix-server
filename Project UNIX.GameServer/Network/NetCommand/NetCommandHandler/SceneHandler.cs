using NMX.Protocal;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.GameManager;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Network.NetCommand.NetAttribute;
using ProjectUNIX.GameServer.Network.NetCommand.Result;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    [NetController]
    internal class SceneHandler : HandlerBase
    {
        [NetCommand(MessageId.EnterSceneReq)]
        public async ValueTask<IResult> EnterSceneReq(GamePlayer player,SceneModuleManager sceneModuleManager)
        {

            EnterSceneRsp rsp = new EnterSceneRsp { Id = 3 };

            return Response(MessageId.EnterSceneRsp,rsp);
        }

        [NetCommand(MessageId.ChestInteractionReq)]
        public async ValueTask<IResult> ChestInteraction(GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            ChestInteractionReq req = Packet!.DecodeBody<ChestInteractionReq>();

            await sceneModuleManager.ChestInteractionNotify((uint)req.ChestId);

            return Response(MessageId.ChestInteractionRsp, new ChestInteractionRsp { });

        }

        [NetCommand(MessageId.EnterScenePreStateReq)]
        public async ValueTask<IResult> EnterScenePreStateReq(GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            sceneModuleManager.OnClientChangedLoadingState(Game.Scene.SceneEnterState.Pre);

            return Response(MessageId.ChestInteractionRsp, new EnterScenePreStateReq { });

        }

        [NetCommand(MessageId.EnterScenePostStateReq)]
        public async ValueTask<IResult> EnterScenePostStateReq(GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            sceneModuleManager.OnClientChangedLoadingState(Game.Scene.SceneEnterState.Post);

            return Response(MessageId.None, new EnterScenePostStateReq { });

        }

        [NetCommand(MessageId.EnterSceneFinishStateReq)]
        public async ValueTask<IResult> EnterSceneFinishStateReq(GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            sceneModuleManager.OnClientChangedLoadingState(Game.Scene.SceneEnterState.Finish);

            return Response(MessageId.None, new EnterSceneFinishStateReq { });

        }

    }
}
