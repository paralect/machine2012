using System;
using System.Collections.Generic;
using Paralect.Domain.Test.Events;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using Paralect.Machine.Processes;
using Paralect.Machine.Processes.Trash;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Domain.Aggregates
{
    public class CreateDeveloper : ICommand<DeveloperId>
    {
        public String Name { get; set; }
    }

//    [Message("adfasdfasdfadf", typeof(ChangeDeveloperName))]
    [ProtoInclude(3, "hello")]
    public class ChangeDeveloperName : ICommand<DeveloperId>
    {
        public String NewName { get;set;}
    }

    //[Message("adfasdfasdfadf")]

    public class DeveloperCreated : IEvent<DeveloperId>
    {
        public String Name { get; set; }
    }

//    [Message("adfasdfasdfadf")]
    public class DeveloperNameChanged : IEvent<DeveloperId>
    {
        public String NewName { get; set; }
    }

    public class DeveloperId : StringId
    {
        public DeveloperId(string value) { Value = value; }
    }

    public class DeveloperState : ProcessState<DeveloperId>
    {
        //[ProtoBuf.ProtoMember(1, ]
        public string Name { get; set; }

        protected void When(DeveloperCreated created)
        {
            Name = created.Name;
        }

        protected void When(DeveloperNameChanged changed)
        {
            Name = changed.NewName;
        }
    }

    public class DeveloperAR : Process<DeveloperId, DeveloperState>
    {
        public IResult Handle(CreateDeveloper create)
        {
            return Apply(new DeveloperCreated() { Name = create.Name, });
        }

        public IResult Handle(ChangeDeveloperName change)
        {
            return Apply(new DeveloperNameChanged() { NewName = change.NewName });
            //return Subscribe<DeveloperId, DeveloperNameChanged>(new DeveloperId("dfdf"));
        }
    }



    public class User : MPowerAggregateRoot
    {
        private String _firstName;
        private String _lastName;
        private String _email;

        /// <summary>
        /// Close access to default constructor for aggregate consumers
        /// </summary>
        private User() {}

        public User(UserCreatedEvent createdEvent)
        {
            Apply(createdEvent);
        }

        private void On(UserCreatedEvent created)
        {
            _id = created.UserId;
            _firstName = created.FirstName;
            _lastName = created.LastName;
            _email = created.Email;
        }

        private void On(UserNameChangedEvent created)
        {
            _firstName = created.FirstName;
            _lastName = created.LastName;
        }

        private void On(UserEmailChangedEvent changed)
        {
            _email = changed.Email;
        }

        

    }
}
