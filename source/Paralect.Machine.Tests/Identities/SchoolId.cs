using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using NUnit.Framework;
using Paralect.Machine.Domain;
using ProtoBuf;
using ProtoBuf.Meta;
using Paralect.Machine.Utils;

namespace Paralect.Machine.Tests.Identities
{
    public class IdentityTest
    {
        public void test_it()
        {

/*
            var data = SerializeProtocalBuffer(person, model);
            var result = DeserializeProtocalBuffer(data, model, typeof(SchoolId));

            Assert.That(result.Address.Line1, Is.EqualTo(person.Address.Line1));
            Assert.That(result.Address.BaseProperty, Is.EqualTo(person.Address.BaseProperty));  */          

        }

        public static byte[] SerializeProtocalBuffer(Object user, RuntimeTypeModel model)
        {
            using (MemoryStream ms = new System.IO.MemoryStream())
            {
                model.Serialize(ms, user);
                return ms.ToArray();
            }
        }

        public static Object DeserializeProtocalBuffer(byte[] data, RuntimeTypeModel model, Type dataType)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                Object user = (model.Deserialize(memoryStream, null, dataType));
                return user;
            }
        }
    }

    public class MongoTransitionDataSerializer
    {
        public Object Deserialize(BsonDocument doc, Type type)
        {
            return BsonSerializer.Deserialize(doc, type);
        }

        public BsonDocument Serialize(Object obj)
        {
            BsonDocument data = new BsonDocument();

            var writer = BsonWriter.Create(data);
            BsonSerializer.Serialize(writer, obj.GetType(), obj);

            return data;
        }
    }

}
