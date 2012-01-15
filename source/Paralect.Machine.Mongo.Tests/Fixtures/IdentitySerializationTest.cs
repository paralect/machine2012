using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Mongo.Tests.Helpers.Mongo;

namespace Paralect.Machine.Mongo.Tests.Fixtures
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

    [EntityTag("4b612ae7-a947-4fb6-8beb-c9a9a9045aa0")]
    public class SchoolId : StringIdentity
    {

        public SchoolId(string value) { Value = value; }
    }

    public class SchoolDocument
    {
        public SchoolId SchoolIdentity { get; set; }
        public String Name { get; set; }
        public Int32 Year { get; set; }
    }

    #endregion
}
