using System;
using System.Collections.Generic;
using Paralect.Domain.Test.Events;
using Paralect.Machine.Domain;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Tests.Areas.Domain.Aggregates
{
    public class CreateDeveloper : Command<DeveloperId>
    {
        public String Name { get; set; }
    }

    public class ChangeDeveloperName : Command<DeveloperId>
    {
        public String NewName { get;set;}
    }


    public class DeveloperCreated : Event<GuidId, DeveloperId, EventMetadata<GuidId, DeveloperId>>
    {
        public String Name { get; set; }
    }

    public class DeveloperNameChanged : Event<GuidId, DeveloperId, EventMetadata<GuidId, DeveloperId>>
    {
        public String NewName { get; set; }
    }

    public class DeveloperId : StringId
    {
        public DeveloperId(string value) { Value = value; }
    }

    public class DeveloperState : AggregateState<DeveloperId>
    {
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

    public class DeveloperAR : AggregateRoot<DeveloperId, DeveloperState>
    {
        public IResult Handle(CreateDeveloper create, DeveloperState state)
        {
            return Apply(new DeveloperCreated() { Name = create.Name, });
        }

        public IResult Handle(ChangeDeveloperName change, DeveloperState state)
        {
            return Apply(new DeveloperNameChanged() { NewName = change.NewName });
        }

        public IResult Handle(ChangeDeveloperName change)
        {
            return Empty();
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
