using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NMX.Protocal;
using Project_UNIX.Common.Server;
using Project_UNIX.Common.Validator.Token;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Game.GameManager;
using ProjectUNIX.GameServer.Game.Player;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Network.NetCommand.NetAttribute;
using ProjectUNIX.GameServer.Network.NetCommand.Result;
using System.Security.Claims;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    [NetController]
    internal class LoginHandler : HandlerBase
    {
        [NetCommand(MessageId.PlayerTokenCs)]
        public ValueTask<IResult> PlayerTokenCs(NetSession session, GamePlayer player, TokenValidation tokenValidation, Account account, ILogger<LoginHandler> logger)
        {
            PlayerTokenCs req = Packet!.DecodeBody<PlayerTokenCs>();

            var tokenDecode = tokenValidation.DecodeJwt(req.Token);

            if (tokenDecode == null)
            {
                logger.LogError("Token invalid");
                session.Close();
            }
            Claim uidClaim = tokenDecode!.Claims.FirstOrDefault(c => c.Type == "userId")!;

            var secret = account.GetAccountByUid(int.Parse(uidClaim.Value)).secret;

            if (!tokenValidation.ValidateJwt(req.Token, secret) && tokenValidation.IsTokenValid(req.Token, secret))
            {
                logger.LogError("Token Error: Validation uncorrectly or Token was expired");
                session.Close();
            }

            player.InitializePlayerDefault(uint.Parse(uidClaim.Value));

            session.ChangeSessionState(NetSessionState.ACTIVE);

            return ValueTask.FromResult(Response(MessageId.PlayerTokenSc, new PlayerTokenSc { }));
        }



        [NetCommand(MessageId.EnterGameReq)]
        public async ValueTask<IResult> EnterGameReq(NetSession session, GamePlayer player, SceneModuleManager sceneModuleManager)
        {
            await session.NotifyAsync(MessageId.ClientInitNotify, new ClientInitNotify { });

            await sceneModuleManager.EnterSceneAsync(3,EnterReason.EnterSelf);

            LoginRsp rsp = new LoginRsp
            {
                Id = (int)player.Id,
            };

            return Response(MessageId.EnterGameRsp, rsp);
        }

        [NetCommand(MessageId.PlayerBreathingReq)]
        public ValueTask<IResult> PlayerBreathing()
        {
            return ValueTask.FromResult(Response(MessageId.PlayerBreathingRsp, new PlayerBreathingRsp
            {
                ServerTime = (uint)DateTimeOffset.Now.ToUnixTimeSeconds()

            }));
        }
    }
}
