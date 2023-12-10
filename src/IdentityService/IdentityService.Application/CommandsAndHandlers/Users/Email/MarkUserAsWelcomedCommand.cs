using System;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email;

public class MarkUserAsWelcomedCommand : ICommand<object>
{
    private Guid guid;
    private Guid userId;

    public MarkUserAsWelcomedCommand(Guid guid, Guid userId)
    {
        this.guid = guid;
        this.userId = userId;
    }

    public Guid Id => Guid.NewGuid();
}