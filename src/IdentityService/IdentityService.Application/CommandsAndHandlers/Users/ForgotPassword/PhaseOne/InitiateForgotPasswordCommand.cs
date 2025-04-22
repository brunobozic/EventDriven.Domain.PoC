using SharedKernel.DomainContracts;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.ForgotPassword.PhaseOne;

public class InitiateForgotPasswordCommand : ICommand<bool>
{
    internal string Email;
    public string Origin;

    private Guid UserId;
    internal string UserName;

    public InitiateForgotPasswordCommand(Guid userId, string email, string userName)
    {
        Email = email;
        UserId = userId;
        UserName = userName;
    }

    public Guid Id => throw new NotImplementedException();
}