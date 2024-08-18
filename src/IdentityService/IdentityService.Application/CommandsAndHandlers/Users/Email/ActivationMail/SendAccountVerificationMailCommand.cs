using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email.ActivationMail;

public class SendAccountVerificationMailCommand : CommandBase<AccountVerificationMailSentDto>
{
    public string ActivationLink;
    public DateTimeOffset? ActivationLinkGenerated;
    public string Email;
    public string FirstName;
    public string LastName;
    public string Origin;
    public string UserName;

    public SendAccountVerificationMailCommand(
        string activationLink,
        DateTimeOffset? activationLinkGenerated,
        string email,
        string userName,
        string firstName,
        string lastName,
        string origin
    )
    {
        ActivationLink = activationLink;
        ActivationLinkGenerated = activationLinkGenerated;
        Email = email;
        FirstName = firstName;
        UserName = userName;
        LastName = lastName;
        Origin = origin;
    }
}