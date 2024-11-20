using Google.Protobuf;
using Microsoft.Extensions.Logging;
using System.Net;
using ProjectUNIX.GameServer.Network.Packet;
using ProjectUNIX.GameServer.Network.Manager;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Network.NetCommand.Result;
using ProjectUNIX.GameServer.Network.NetCommand;
using ProjectUNIX.GameServer.Game.World;

namespace ProjectUNIX.GameServer.Network.kcp.Session
{
    internal abstract class NetSession : IDisposable
    {

        public IPEndPoint EndPoint => networkUnit!.RemoteEndPoint;

        public long SessionId { get; private set; }

        public string SessionKey { get; private set; }

        protected INetworkUnit networkUnit;

        public NetSessionState SessionState { get; protected set; }

        private readonly NetSessionManager sessionManager;

        private readonly NetCommandDispatcher _commandDispatcher;

        private readonly ILogger logger;


        public NetSession(ILogger<NetSession> logger, NetSessionManager sessionManager, NetCommandDispatcher commandDispatcher)
        {
            this.logger = logger;
            this.sessionManager = sessionManager;
            _commandDispatcher = commandDispatcher;
        }

        public abstract ValueTask RunAsync();

        public abstract void Close();

        public abstract ValueTask SendAsync(NetPacket packet);

        public void Establish(long sessionId, INetworkUnit networkUnit)
        {
            SessionId = sessionId;
            this.networkUnit = networkUnit;

            sessionManager.Add(this);
        }

        public async Task NotifyAsync<TNotify>(MessageId cmdType, TNotify notify) where TNotify : IMessage<TNotify>
        {
            await SendAsync(new()
            {
                CmdType = cmdType,
                Head = Memory<byte>.Empty,
                Body = notify.ToByteArray()
            });
        }

        public virtual void Dispose()
        {
            networkUnit?.Dispose();
            _ = sessionManager.TryRemove(this);
        }
        protected async ValueTask<int> PacketHandlerAsync(Memory<byte> buffer)
        {
            if (buffer.Length < 12)
                return 0;

            int consumed = 0;
            do
            {
                (NetPacket? packet, int bytesConsumed) = NetPacket.DecodeFrom(buffer[consumed..]);
                consumed += bytesConsumed;

                if (packet == null)
                    return consumed;

                IResult? result = await _commandDispatcher.InvokeHandler(packet);
                if (result != null)
                {
                    while (result.NextPacket(out NetPacket? serverPacket))
                    {
                        await SendAsync(serverPacket);
                    }

                    //logger.LogInformation("Successfully handled command of type {cmdType}", packet.CmdType);
                }

            } while (buffer.Length - consumed >= 12);

            return consumed;
        }

        protected async ValueTask<int> ReadWithTimeoutAsync(Memory<byte> buffer, int timeoutSeconds)
        {
            using CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(timeoutSeconds));
            return await networkUnit!.ReceiveAsync(buffer, cancellationTokenSource.Token);
        }

        protected async ValueTask WriteWithTimeoutAsync(Memory<byte> buffer, int timeoutSeconds)
        {
            using CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(timeoutSeconds));
            await networkUnit!.SendAsync(buffer, cancellationTokenSource.Token);
        }

        public void ChangeSessionState(NetSessionState newState)
        {
            SessionState = newState;
        }
    }
}
