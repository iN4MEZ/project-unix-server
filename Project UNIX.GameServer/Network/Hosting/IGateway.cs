using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUNIX.GameServer.Network.Hosting
{
    internal interface IGateway
    {
        Task Run();
        Task Stop();
    }
}
