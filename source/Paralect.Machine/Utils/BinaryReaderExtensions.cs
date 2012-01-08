using System;
using System.IO;
using MongoDB.Bson;

namespace Paralect.Machine.Utils
{
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a GUID value from the current stream and advances the current position of the stream by sixteen bytes.
        /// </summary>
        public static Guid ReadGuid(this BinaryReader reader)
        {
            // Precaching Guid bytes size assuming it wouldn't change
            // Guid.NewGuid().ToByteArray().Length == 16
            const int GuidLength = 16;

            byte[] bytes = reader.ReadBytes(GuidLength);
            return new Guid(bytes);
        }

        /// <summary>
        /// Reads an MongoDB ObjectId value from the current stream and advances the current position of the stream by twelve bytes.
        /// </summary>
        public static ObjectId ReadObjectId(this BinaryReader reader)
        {
            // Precaching Guid bytes size assuming it wouldn't change
            // ObjectId.Empty.ToByteArray().Length == 12
            const int ObjectIdLength = 12;

            byte[] bytes = reader.ReadBytes(ObjectIdLength);
            return new ObjectId(bytes);
        }
    }
}