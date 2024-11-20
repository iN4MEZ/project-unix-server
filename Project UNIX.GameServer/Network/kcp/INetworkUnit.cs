using System.Net;

namespace ProjectUNIX.GameServer.Network.kcp
{
    internal interface INetworkUnit : IDisposable
    {
        IPEndPoint RemoteEndPoint { get; }

        ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken);
        ValueTask SendAsync(Memory<byte> buffer, CancellationToken cancellationToken);
    }
}
