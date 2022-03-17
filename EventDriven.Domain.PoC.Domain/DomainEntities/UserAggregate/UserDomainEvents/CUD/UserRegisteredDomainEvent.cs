using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using EventDriven.Domain.PoC.SharedKernel.Helpers;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserRegisteredDomainEvent : DomainEventBase
    {
        public DateTimeOffset? DateOfBirth;

        public string Email;
        public string FirstName;
        public string LastName;
        public string Oib;
        public RoleEnum Role;
        public string UserEmail;
        public Guid UserId;
        public string UserName;
        public string UserRole;

        public UserRegisteredDomainEvent(
            string email
            , string userName
            , string userEmail
            , string firstName
            , string lastName
            , string role
            , Guid userId
            , string oib
            , DateTimeOffset? dateOfBirth
        )
        {
            Email = email;
            UserName = userName;
            UserEmail = userEmail;
            FirstName = firstName;
            LastName = lastName;
            UserRole = role;
            UserId = userId;
            Oib = oib;
            DateOfBirth = dateOfBirth;
        }
    }
}