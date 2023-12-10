using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Handlers;

public class RoleDeletedDomainEventHandler : INotificationHandler<RoleDeletedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public RoleDeletedDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(RoleDeletedNotification notification, CancellationToken cancellationToken)
    {
    }
}