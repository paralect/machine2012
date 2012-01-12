using NUnit.Framework;
using Paralect.Machine.Tests.Helpers;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Tests.Fixtures.Protobuf
{
    [TestFixture]
    public class InheritanceSerializationTest
    {
        [Test]
        public void should_serialize_and_deserialize_inheritance()
        {
            RuntimeTypeModel model = TypeModel.Create();

            model[typeof (Guru)]
                .AddSubType(10000, typeof (Developer))
                .AddSubType(10001, typeof (Designer));

            var guru = new Guru {Id = 67, Name = "Misha"};
            AssertSerializedAndDeserialized(guru, model);

            var developer = new Developer {Id = 56, Name = "Tolyan", ProgrammingLanguage = "COBOL"};
            AssertSerializedAndDeserialized(developer, model);
            AssertSerializedAndDeserialized<Guru>(developer, model);

            var designer = new Designer {Id = 77, Name = "Dimon", FavoriteColor = "Oyaebu"};
            AssertSerializedAndDeserialized(designer, model);
            AssertSerializedAndDeserialized<Guru>(designer, model);
        }

        /// <summary>
        /// Serializes, decerializes and checks that objects are equal 
        /// </summary>
        private void AssertSerializedAndDeserialized<TObject>(TObject obj, RuntimeTypeModel model)
        {
            byte[] bytes = ProtobufSerializer.SerializeProtocalBuffer(obj, model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<TObject>(bytes, model);

            bool result = ObjectComparer.AreObjectsEqual(obj, back);
            Assert.That(result, Is.True);
        }

    }


    #region Helper classes

    [ProtoContract]
    public class Guru
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }

    [ProtoContract]
    public class Developer : Guru
    {
        [ProtoMember(1)]
        public string ProgrammingLanguage { get; set; }
    }

    [ProtoContract]
    public class Designer : Guru
    {
        [ProtoMember(1)]
        public string FavoriteColor { get; set; }
    }

    #endregion
}