using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Paralect.Machine.Domain;

namespace Paralect.Machine.Mongo
{
    public class IdentitySerializer : BsonBaseSerializer
    {
        /// <summary>
        /// Register this serializer with specified identities types. 
        /// There is no way to unregister! Because BsonSerializer simply doesn't support this. 
        /// Operation is idempotant, so you can call it as many times as you want.
        /// </summary>
        public static void RegisterForIdentityTypes(IEnumerable<Type> identityTypes)
        {
            foreach (var identityType in identityTypes)
            {
                BsonSerializer.RegisterSerializer(identityType, new IdentitySerializer()); // can we use single serializer? will it be thread safe?                
            }
        }

        /// <summary>
        /// Register this serializer with specified identities types. 
        /// Overload method.
        /// </summary>
        public static void RegisterForIdentityTypes(params Type[] identityTypes)
        {
            RegisterForIdentityTypes((IEnumerable<Type>) identityTypes);
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            IIdentity id = (IIdentity) value;
            bsonWriter.WriteString(id.Value);
        }

        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            var val = bsonReader.ReadString();
            return Activator.CreateInstance(nominalType, val);
        }

        public override object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            return Deserialize(bsonReader, nominalType, null, options);
        }
    }
}
