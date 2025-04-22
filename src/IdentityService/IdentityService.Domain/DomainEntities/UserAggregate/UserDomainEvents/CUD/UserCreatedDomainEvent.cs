using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;

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
        , EventTypeEnum typeOfEvent
    )
    {
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Oib = oib;
        DateOfBirth = dateOfBirth;
        ActivationLinkGenerated = activationLinkGenerated;
        ActivationLink = activationLink;
        CreatorUserId = creatorUserId;
        Origin = origin;
        UserResourceId = userResourceId;
        TypeOfEvent = typeOfEvent;
    }
}