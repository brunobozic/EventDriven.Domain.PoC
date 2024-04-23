using SharedKernel.DomainContracts;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email.ActivationMail;

public class ResendAccountVerificationEmailCommand : ICommand<bool>
{
    public string Email;
    public string FirstName;
    public string LastName;
    public string Origin;
    public Guid UserId;
    public string UserName;

    public ResendAccountVerificationEmailCommand(string email, string userName)
    {
        Email = email;
        UserName = userName;
    }

    public Guid Id => Guid.NewGuid();
}