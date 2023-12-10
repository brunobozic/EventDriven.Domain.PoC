using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.CommandsAndHandlers.Users.Email;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Registration;

public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public UserRegisteredEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        await _commandsScheduler.EnqueueAsync(new MarkUserAsWelcomedCommand(
            Guid.NewGuid(),
            notification.UserId));
    }
}