using Google.Protobuf;
using Project_UNIX.Protocol;
using ProjectUNIX.GameServer.Network.NetCommand.Result;
using ProjectUNIX.GameServer.Network.Packet;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetCommandHandler
{
    internal abstract class HandlerBase
    {
        public NetPacket? Packet { get; set; }

        protected IResult Ok()
        {
            return new SinglePacketResult(null);
        }

        protected IResult Response<TMessage>(MessageId cmdType, TMessage message) where TMessage : IMessage
        {
            return new SinglePacketResult(new()
            {
                CmdType = cmdType,
                Head = Memory<byte>.Empty,
                Body = message.ToByteArray()
            });
        }
    }
}
