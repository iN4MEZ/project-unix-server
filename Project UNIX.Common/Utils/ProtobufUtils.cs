using Google.Protobuf;


namespace Project_UNIX.Common.Utils
{

    public static class ProtobufUtils
    {
        public static byte[] Serialize<T>(T obj) where T : IMessage<T>
        {
            return obj.ToByteArray();
        }

        public static T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
        {
            T message = new T();
            message.MergeFrom(data);
            return message;
        }
    }
}
