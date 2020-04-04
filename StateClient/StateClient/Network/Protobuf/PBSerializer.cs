using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using StateClient.Network.Protobuf;
namespace StateClient.Network.Protobuf
{
    public class PBSerializer
    {

        //public static byte[] serialize<T>(T obj)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    ProtoBuf.Serializer.Serialize(stream, obj);
        //    return stream.ToArray();

        //}
        //public static NetworkMsg deserialize(byte[] bytes)
        //{
        //    MemoryStream stream = new MemoryStream(bytes);

        //    return Serializer.Deserialize<NetworkMsg>(stream);
        //}
        public static byte[] serialize<T>(T obj) where T : IMessage
        {
            byte[] data = obj.ToByteArray();
            return data;
        }

        public static T deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);
            return message as T;
        }
    }
}
