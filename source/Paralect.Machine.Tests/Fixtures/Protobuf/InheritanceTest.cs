using NUnit.Framework;
using Paralect.Machine.Domain;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;
using ProtoBuf.Meta;
using mPower.Accounting.Tests;

namespace Paralect.Machine.Tests.Fixtures.Protobuf
{
    [TestFixture]
    public class InheritanceTest
    {
        [Test]
         public void should_serialize_and_deserialize_inheritance()
         {
             var model = TypeModel.Create();
             
             model[typeof(Guru)]
                 .AddSubType(10000, typeof(Developer))
                 .AddSubType(10001, typeof(Designer));

             var guru = new Guru() { Id = 67, Name = "Misha" };
             AssertSerializedAndDeserialized(guru, model);

             var developer = new Developer() { Id = 56, Name = "Tolyan", ProgrammingLanguage = "COBOL" };
             AssertSerializedAndDeserialized<Developer>(developer, model);
             AssertSerializedAndDeserialized<Guru>(developer, model);

             var designer = new Designer() { Id = 77, Name = "Dimon", FavoriteColor = "Oyaebu" };
             AssertSerializedAndDeserialized<Designer>(designer, model);
             AssertSerializedAndDeserialized<Guru>(designer, model);
         }

        private void AssertSerializedAndDeserialized<TObject>(TObject obj, RuntimeTypeModel model)
        {
            var bytes = ProtobufSerializer.SerializeProtocalBuffer(obj, model);
            var back = ProtobufSerializer.DeserializeProtocalBuffer<TObject>(bytes, model);

            var result = ObjectComparer.AreObjectsEqual(obj, back);
            Assert.That(result, Is.True);
        }

    }


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
}