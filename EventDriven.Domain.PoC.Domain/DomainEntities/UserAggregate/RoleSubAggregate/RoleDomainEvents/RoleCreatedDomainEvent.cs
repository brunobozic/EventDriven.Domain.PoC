using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleCreatedDomainEvent : DomainEventBase
    {
        public string CreatorEmail;
        public Guid CreatorId;
        public string CreatorUsername;
        public DateTimeOffset? DateCreated;
        public string Description;
        public string Name;
        public RoleId RoleId;

        public RoleCreatedDomainEvent(
            string name
            , string description
            , Guid roleId
            , Guid creatorId
            , string creatorUsername
            , string creatorEmail
            , DateTimeOffset dateCreated
        )
        {
            RoleId = new RoleId(roleId);
            Name = name;
            Description = description;
            CreatorId = creatorId;
            CreatorUsername = creatorUsername;
            CreatorEmail = creatorEmail;
            DateCreated = dateCreated;
        }
    }
}