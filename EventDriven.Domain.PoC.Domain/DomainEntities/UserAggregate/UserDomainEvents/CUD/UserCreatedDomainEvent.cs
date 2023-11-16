using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public Guid? CreatorUserId;
        public DateTimeOffset? DateOfBirth;
        public string Email;
        public string FirstName;
        public string LastName;
        public string Oib;
        public string Origin;
        public Guid UserId;
        public string UserName;
        public Guid UserResourceId;

        public UserCreatedDomainEvent(
            Guid userId
            , string email
            , string userName
            , string firstName
            , string lastName
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset? activationLinkGenerated
            , string activationLink
            , Guid? creatorUserId
            , Guid userResourceId
            , string origin
        )
        {
            Email = email;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            Oib = oib;
            DateOfBirth = dateOfBirth;
            ActivationLinkGenerated = activationLinkGenerated;
            ActivationLink = activationLink;
            CreatorUserId = creatorUserId;
            Origin = origin;
            UserResourceId = userResourceId;
        }
    }
}