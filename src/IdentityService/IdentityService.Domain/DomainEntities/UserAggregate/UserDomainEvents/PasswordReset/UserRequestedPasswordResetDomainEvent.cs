using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.PasswordReset;

public class UserRequestedPasswordResetDomainEvent : DomainEventBase
{
    public UserRequestedPasswordResetDomainEvent(string email, string userName, Guid id, string randomTokenString)
    {
        Email = email;
        UserName = userName;
        Id = id;
        RandomTokenString = randomTokenString;
    }

    public string Email { get; }
    public string UserName { get; }
    public Guid Id { get; }
    public string RandomTokenString { get; }
}