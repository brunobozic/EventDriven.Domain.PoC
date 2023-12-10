using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.EventsAndEventHandlers.Roles.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Handlers;

public class RoleAssignedToUserDomainEventHandler : INotificationHandler<RoleAssignedToUserNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public RoleAssignedToUserDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(RoleAssignedToUserNotification notification, CancellationToken cancellationToken)
    {
        // TODO: journal
    }
}