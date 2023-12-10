using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.CommandsAndHandlers.Users.Email;
using IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Handlers;

public class UserReactivatedDomainEventHandler : INotificationHandler<UserReactivatedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public UserReactivatedDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(UserReactivatedNotification notification, CancellationToken cancellationToken)
    {
        // Send welcome e-mail message...

        await _commandsScheduler.EnqueueAsync(new MarkUserAsWelcomedCommand(
            Guid.NewGuid(),
            notification.UserId));
    }
}