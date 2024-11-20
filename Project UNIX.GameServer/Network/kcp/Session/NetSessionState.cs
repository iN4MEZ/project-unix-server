using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Network.kcp.Session
{
    enum NetSessionState
    {
        WAIT_FOR_TOKEN,
        ACTIVE,
        UNACTIVE,
    }
}
