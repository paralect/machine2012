using System;
using System.IO;
using ProtoBuf.Meta;

namespace Paralect.Machine.Tests.Helpers.Protobuf
{
    public class ProtobufSerializer
    {
        public static byte[] SerializeProtocalBuffer(Object obj, RuntimeTypeModel model)
        {
            using (MemoryStream ms = new System.IO.MemoryStream())
            {
                model.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static TObject DeserializeProtocalBuffer<TObject>(byte[] data, RuntimeTypeModel model)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                TObject user = (TObject)(model.Deserialize(memoryStream, null, typeof(TObject)));
                return user;
            }
        }

        /// <summary>
        /// Overload.
        /// </summary>
        public static byte[] SerializeProtocalBuffer(Object obj)
        {
            return SerializeProtocalBuffer(obj, RuntimeTypeModel.Default);
        }

        /// <summary>
        /// Overload.
        /// </summary>
        public static TObject DeserializeProtocalBuffer<TObject>(byte[] data)
        {
            return DeserializeProtocalBuffer<TObject>(data, RuntimeTypeModel.Default);
        }
    }
}