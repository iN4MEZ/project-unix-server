using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUNIX.GameServer.Network.Packet;

namespace ProjectUNIX.GameServer.Network.NetCommand.Result
{
    internal class SinglePacketResult : IResult
    {
        private NetPacket? _packet;
        public SinglePacketResult(NetPacket? packet)
        {
            _packet = packet;
        }
        public bool NextPacket([MaybeNullWhen(false)] out NetPacket packet)
        {
            packet = _packet;
            _packet = null;

            return packet != null;
        }
    }
}
