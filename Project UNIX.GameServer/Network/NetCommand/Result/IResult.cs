using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUNIX.GameServer.Network.Packet;

namespace ProjectUNIX.GameServer.Network.NetCommand.Result
{
    internal interface IResult
    {
        bool NextPacket([MaybeNullWhen(false)] out NetPacket packet);
    }
}
