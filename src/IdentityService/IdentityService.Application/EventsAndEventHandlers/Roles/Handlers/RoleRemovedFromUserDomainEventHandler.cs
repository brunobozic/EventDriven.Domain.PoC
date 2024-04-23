using IdentityService.Application.EventsAndEventHandlers.Roles.Notifications;
using MediatR;
using SharedKernel.DomainContracts;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Handlers;

public class RoleRemovedFromUserDomainEventHandler : INotificationHandler<RoleRemovedFromUserNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public RoleRemovedFromUserDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(RoleRemovedFromUserNotification notification, CancellationToken cancellationToken)
    {
        // TODO: journal
    }
}