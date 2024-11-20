using Project_UNIX.Protocol;

namespace ProjectUNIX.GameServer.Network.NetCommand.NetAttribute
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class NetCommandAttribute : Attribute
    {
        public MessageId CmdType { get; }

        public NetCommandAttribute(MessageId cmdType)
        {
            CmdType = cmdType;
        }
    }
}
