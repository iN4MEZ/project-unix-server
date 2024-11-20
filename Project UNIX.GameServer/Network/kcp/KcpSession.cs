using Microsoft.Extensions.Logging;
using ProjectUNIX.GameServer.Network.Packet;
using ProjectUNIX.GameServer.Network.Manager;
using ProjectUNIX.GameServer.Network.NetCommand;
using ProjectUNIX.GameServer.Network.kcp.Session;
using ProjectUNIX.GameServer.Game.World;

namespace ProjectUNIX.GameServer.Network.kcp
{
    internal class KcpSession : NetSession
    {
        private const int MaxPacketSize = 32768;
        private const int ReadTimeout = 10;
        private const int WriteTimeout = 10;

        private readonly byte[] _recvBuffer;
        private readonly byte[] _sendBuffer;

        private readonly ILogger _logger;

        

        public KcpSession(ILogger<NetSession> logger, NetSessionManager sessionManager, NetCommandDispatcher commandDispatcher) : base(logger, sessionManager, commandDispatcher)
        {
            _recvBuffer = GC.AllocateUninitializedArray<byte>(MaxPacketSize);
            _sendBuffer = GC.AllocateUninitializedArray<byte>(MaxPacketSize);

            _logger = logger;
        }

        public override async ValueTask RunAsync()
        {

            SessionState = NetSessionState.WAIT_FOR_TOKEN;

            Memory<byte> recvBuffer = _recvBuffer.AsMemory();

            while (true)
            {
                int readAmount = await ReadWithTimeoutAsync(recvBuffer, ReadTimeout);

                if (readAmount < 0)
                {
                    break;
                }

                int consumedBytes = await PacketHandlerAsync(recvBuffer[..readAmount]);
                if (consumedBytes == -1)
                    break;
            }
        }

        public override async ValueTask SendAsync(NetPacket packet)
        {
            Memory<byte> buffer = _sendBuffer.AsMemory();

            int length = packet.EncodeTo(buffer);

            await WriteWithTimeoutAsync(buffer[..length], WriteTimeout);
        }

        public override void Close()
        {
            Dispose();
        }
    }
}
