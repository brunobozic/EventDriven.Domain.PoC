using System;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email;

public class SendAccountAlreadyRegisteredMailReadiedCommand : ICommand<bool>
{
    private Guid guid;
    private Guid userId;

    public SendAccountAlreadyRegisteredMailReadiedCommand(Guid guid, Guid userId)
    {
        this.guid = guid;
        this.userId = userId;
    }

    public SendAccountAlreadyRegisteredMailReadiedCommand(Guid guid, long userId)
    {
        this.guid = guid;
        UserId = userId;
    }

    public long UserId { get; }

    public Guid Id { get; }
}