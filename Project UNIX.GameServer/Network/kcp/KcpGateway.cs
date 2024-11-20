using NMX.Kcp;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using ProjectUNIX.GameServer.Network.Hosting;
using ProjectUNIX.GameServer.Network.Manager;
using ProjectUNIX.GameServer.Kcp;

namespace ProjectUNIX.GameServer.Network.kcp
{
    internal class KcpGateway : IGateway
    {
        private IKcpTransport<IKcpMultiplexConnection> kcpTransport;

        private readonly Random rand;

        private readonly NetSessionManager sessionManager;

        private readonly ILogger logger;
		

        private uint sessionCounter;

        private int port;

        public KcpGateway(ILogger<KcpGateway> logger, NetSessionManager sessionManager)
        {
            rand = new Random();
            port = 13720;
            this.logger = logger;
            this.sessionManager = sessionManager;

        }
        public Task Run()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            kcpTransport = KcpSocketTransport.CreateMultiplexConnection(new(endPoint), 1400);

            kcpTransport.SetCallbacks(20, HandShake);

            kcpTransport.Start();

            logger.LogInformation("Server Listen on {endpoint}", endPoint);

            return Task.CompletedTask;
        }

        private async ValueTask HandShake(UdpReceiveResult udpReceiveResult)
        {
            KcpHandShake handshake = KcpHandShake.ReadFrom(udpReceiveResult.Buffer);

            switch ((handshake.Head, handshake.Tail))
            {
                case (KcpHandShake.StartConversationHead, KcpHandShake.StartConversationTail):
                    logger.LogInformation("{udpReceiveResult.RemoteEndPoint} has allow to conversation", udpReceiveResult.RemoteEndPoint);
                    await OnStartConversationRequest(udpReceiveResult.RemoteEndPoint);
                    break;
            }
        }

        private async ValueTask OnStartConversationRequest(IPEndPoint endpoint)
        {

            //if(sessionManager.GetSessionEstablishByEndPoint(endpoint))
            //{
            //    logger.LogInformation("{endpojnt} Trying to Connect as same Address",endpoint );
            //    return;
            //}

            uint convId = Interlocked.Increment(ref sessionCounter);
            uint token = (uint)rand.Next();

            long convId64 = convId << 32 | token;


            KcpConversation kcpConv = kcpTransport!.Connection.CreateConversation(convId64, endpoint);

            _ = sessionManager.RunSessionAsync(convId64, new KcpNetworkUnit(kcpConv, endpoint));

            await SendConversationCreatedPacket(endpoint, convId, token);
        }

        private async Task SendConversationCreatedPacket(IPEndPoint clientEndPoint, uint convId, uint token)
        {
            KcpHandShake handshakeResponse = new()
            {
                Head = KcpHandShake.ConversationCreatedHead,
                Param1 = convId,
                Param2 = token,
                Data = 1234567890,
                Tail = KcpHandShake.ConversationCreatedTail
            };

            byte[] buffer = ArrayPool<byte>.Shared.Rent(20);
            try
            {
                Memory<byte> bufferMemory = buffer.AsMemory();

                handshakeResponse.WriteTo(buffer);

                await kcpTransport!.SendPacketAsync(bufferMemory[..20], clientEndPoint, CancellationToken.None);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}
