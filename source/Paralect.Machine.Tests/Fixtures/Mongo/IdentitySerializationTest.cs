using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using Paralect.Machine.Domain;
using Paralect.Machine.Mongo;
using Paralect.Machine.Tests.Helpers.Mongo;
using ProtoBuf;

namespace Paralect.Machine.Tests.Fixtures.Mongo
{
    [TestFixture]
    public class IdentitySerializationTest
    {
        [Test]
        public void should_throw_if_serialized_directly()
        {
            IdentitySerializer.RegisterForIdentityTypes(typeof(SchoolId));

            var schoolId = new SchoolId("school-56");

            Assert.Throws<InvalidOperationException>(() =>
            {
                MongoSerializer.Serialize(schoolId);
            });
        }

        [Test]
        public void should_serialize_and_deserialize()
        {
            IdentitySerializer.RegisterForIdentityTypes(typeof(SchoolId));

            var doc = new SchoolDocument()
            {
                Name = "Minsk",
                Year = 2012,
                SchoolIdentity = new SchoolId("school-67")
            };

            var bson = MongoSerializer.Serialize(doc);
            var idElement = bson["SchoolIdentity"];
            Assert.That(idElement.IsString, Is.True);
            Assert.That(idElement.AsString, Is.EqualTo(doc.SchoolIdentity.Value));

            var back = (SchoolDocument) MongoSerializer.Deserialize(bson, typeof (SchoolDocument));

            Assert.That(back.SchoolIdentity.Value, Is.EqualTo(doc.SchoolIdentity.Value));
            Assert.That(back.Name, Is.EqualTo(doc.Name));
            Assert.That(back.Year, Is.EqualTo(doc.Year));
        }

    }


    #region Helper classes

    [EntityTag(1)]
    public class SchoolId : StringIdentity
    {
        [ProtoMember(1)]
        public override sealed string Value { get; protected set; }

        public SchoolId(string value) : base(value) { }
    }

    public class SchoolDocument
    {
        public SchoolId SchoolIdentity { get; set; }
        public String Name { get; set; }
        public Int32 Year { get; set; }
    }

    #endregion
}
