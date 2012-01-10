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
using Paralect.Machine.Tests.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;
using Paralect.Machine.Utils;

namespace Paralect.Machine.Tests.Identities
{
    [EntityTag(1)]
    public class SchoolId : StringIdentity
    {
        [ProtoMember(1)] 
        public override sealed string Value { get; protected set; }

        public SchoolId(string value) : base(value) { }
    }

    public class Outmost
    {
        [BsonRepresentation(BsonType.String)]
        public SchoolId SchoolId { get; set; }
        public String Example { get; set; }
        public Int32 Hello { get; set; }
    }

    public class MyClassSerializer : BsonBaseSerializer 
    {
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
    }

    public class IdentityTest
    {
        public void test_it_2()
        {
            IdentityFactory.Init();
            var tag = IdentityFactory.GetTag(typeof(SchoolId));

            var schoolId = new SchoolId("asdfadf");
            //schoolId.SetPrivatePropertyValue("Value", "hehehehe");

            var data = SerializeProtocalBuffer(schoolId, RuntimeTypeModel.Default);
            var result = DeserializeProtocalBuffer(data, RuntimeTypeModel.Default, typeof(SchoolId));

            var outmost = new Outmost() { Example = "muahaha", Hello = 34, SchoolId = schoolId };

            BsonSerializer.RegisterSerializer(typeof(SchoolId), new MyClassSerializer());


            MongoTransitionDataSerializer serializer = new MongoTransitionDataSerializer();
            var doc = serializer.Serialize(outmost);

            var js = doc.ToJson();

            var back = serializer.Deserialize(doc, typeof(Outmost));
        }



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

    /*
    public class SchoolId : IIdentity 
    {
        private String _schoolId;
        private String _schoolDistrictId;

        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        public string GetId()
        {
            return null;
        }

        /// <summary>
        /// Unique tag (should be unique within the assembly) to distinguish
        /// between different identities, while deserializing.
        /// </summary>
        public string EntityTag
        {
            get { throw new NotImplementedException(); }
        }
    }*/


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
