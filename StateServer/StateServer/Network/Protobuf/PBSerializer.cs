using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
namespace StateServer.Network.Protobuf
{
    public class PBSerializer
    {
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
