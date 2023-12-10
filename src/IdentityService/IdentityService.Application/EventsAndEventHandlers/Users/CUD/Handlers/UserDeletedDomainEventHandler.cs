using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Handlers;

public class UserDeletedDomainEventHandler : INotificationHandler<UserDeletedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public UserDeletedDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(UserDeletedNotification notification, CancellationToken cancellationToken)
    {
    }
}