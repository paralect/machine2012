using System;
using NUnit.Framework;
using Paralect.Machine.Identities;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Tests.Fixtures.Protobuf
{
    [TestFixture]
    public class IdentitySerializationTest
    {
        [Test]
        public void should_serialize_and_deserialize_type_with_identity_field()
        {
            var school = new School()
            {
                Id = new SchoolId("school-49"),
                Name = "Minsk",
                Year = 56,
            };

            var bytes = ProtobufSerializer.SerializeProtocalBuffer(school);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<School>(bytes);

            Assert.That(back.Id.Value, Is.EqualTo(school.Id.Value));
            Assert.That(back.Name, Is.EqualTo(school.Name));
            Assert.That(back.Year, Is.EqualTo(school.Year));

        }
    }

    #region Helper classes

    [EntityTag("d9713c53-b87b-45e2-a669-403b65ca0590")]
    public class SchoolId : StringIdentity
    {
        [ProtoMember(1)]
        public override sealed string Value { get; protected set; }

        public SchoolId(string value) : base(value) { }
    }

    [ProtoContract]
    public class School
    {
        [ProtoMember(1)]
        public SchoolId Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public Int32 Year { get; set; }
    }

    #endregion
}
