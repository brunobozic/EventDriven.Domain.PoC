using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public long? CreatorUserId;
        public DateTimeOffset? DateOfBirth;
        public string Email;
        public string FirstName;
        public string LastName;
        public string Oib;
        public UserId UserId;
        public string UserName;

        public UserCreatedDomainEvent(
            string email
            , string userName
            , string firstName
            , string lastName
            , Guid userId
            , string oib
            , DateTimeOffset? dateOfBirth
            , DateTimeOffset? activationLinkGenerated
            , string activationLink
            , long? creatorUserId
        )
        {
            Email = email;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            UserId = new UserId(userId);
            Oib = oib;
            DateOfBirth = dateOfBirth;
            ActivationLinkGenerated = activationLinkGenerated;
            ActivationLink = activationLink;
            CreatorUserId = creatorUserId;
        }
    }
}