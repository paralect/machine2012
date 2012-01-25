using System;
using System.Linq;
using NUnit.Framework;
using Paralect.Machine.Identities;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Serialization.Fixtures
{
    
    [TestFixture]
    public class IdentityFactoryTests
    {
        [Test]
        public void should_throw_on_two_identity_types_with_the_same_identity_tag()
        {
            Assert.Throws<IdentityTagAlreadyRegistered>(() =>
            {
                new IdentityFactory(typeof(IdentityFactory_Duplicate_ProcessId), typeof(IdentityFactory_Duplicate2_ProcessId));
            });
        }

        [Test]
        public void should_correctly_handle_duplicate_registration_of_the_same_identity()
        {
            var factory = new IdentityFactory(typeof(IdentityFactory_Duplicate_ProcessId), typeof(IdentityFactory_Duplicate_ProcessId));

            Assert.That(factory.IdentityDefinitions.Count(), Is.EqualTo(1));
            Assert.That(factory.IdentityDefinitions.First().Type, Is.EqualTo(typeof(IdentityFactory_Duplicate_ProcessId)));
            Assert.That(factory.IdentityDefinitions.First().Tag, Is.EqualTo(Guid.Parse("30666dff-550b-4e0b-91bf-9f09f82e42c8")));
        }

        [Test]
        public void should_throw_if_no_identity_attribute_for_identity_type()
        {
            Assert.Throws<IdentityTagNotSpecified>(() =>
            {
                new IdentityFactory(typeof(IdentityFactory_WithoutAttribute_ProcessId));
            });                        
        }
    }



    #region Duplicate tag

    [ProtoContract, Identity("{30666dff-550b-4e0b-91bf-9f09f82e42c8}")]
    public class IdentityFactory_Duplicate_ProcessId : StringId { }

    [ProtoContract, Identity("{30666dff-550b-4e0b-91bf-9f09f82e42c8}")]
    public class IdentityFactory_Duplicate2_ProcessId : StringId { }

    #endregion


    #region Message not decorated with MessageAttribute

    [ProtoContract]
    public class IdentityFactory_WithoutAttribute_ProcessId : StringId { }

    #endregion

}