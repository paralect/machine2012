using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Paralect.Machine.Domain;
using ProtoBuf;
using System.IO;
using ProtoBuf.Meta;


namespace Paralect.Machine.Tests.Protobuf
{
    [ProtoContract]
    public class Person 
    {
        [ProtoMember(1)]
        public int Id {get;set;}
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public Address Address {get;set;}
    }

    [ProtoContract]
    public class BaseAddress
    {
        [ProtoMember(1)]
        public string BaseProperty { get; set; }
    }

    [ProtoContract]
    public class Address : BaseAddress
    {
        [ProtoMember(1)]
        public string Line1 {get;set;}
        [ProtoMember(10000)]
        public string Line2 {get;set;}
    }

    public class ProtoTest
    {
        public void test_it()
        {
            var model = TypeModel.Create();
            model[typeof(BaseAddress)].AddSubType(10000, typeof(Address));

/*
            var type = model.Add(typeof(BaseAddress), true);
            //var subTypeA = model.Add(typeof(Address), true);
            type.AddSubType(10000, typeof(Address));
*/

            Person person = new Person
            {
                Address = new Address() { Line1 = "1", Line2 = "2", BaseProperty = "muahaha!"}, Id = 55, Name = "test name"
            };

            var data = SerializeProtocalBuffer(person, model);
            var result = DeserializeProtocalBuffer(data, model);

            Assert.That(result.Address.Line1, Is.EqualTo(person.Address.Line1));
            Assert.That(result.Address.BaseProperty, Is.EqualTo(person.Address.BaseProperty));
        }

        public static byte[] SerializeProtocalBuffer(Person user, RuntimeTypeModel model)
        {
            using (MemoryStream ms = new System.IO.MemoryStream()) 
            {
                model.Serialize(ms, user); 
                return ms.ToArray();
            }
        }

        public static Person DeserializeProtocalBuffer(byte[] data, RuntimeTypeModel model) 
        {
            using (MemoryStream memoryStream = new MemoryStream(data)) 
            {
                Person user = (Person) (model.Deserialize(memoryStream, null, typeof(Person))); 
                return user;
            }
        }
    }
}
