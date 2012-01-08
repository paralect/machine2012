using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Paralect.Machine.Utils
{
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Writes GUID bytes array to the underlying stream
        /// </summary>
        public static void Write(this BinaryWriter writer, Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            writer.Write(bytes);
        }

        /// <summary>
        /// Writes MongoDB ObjectId bytes array to the underlying stream
        /// </summary>
        public static void Write(this BinaryWriter writer, ObjectId objectId)
        {
            byte[] bytes = objectId.ToByteArray();
            writer.Write(bytes);
        }
    }
}
