using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Handlers;

public class RoleCreatedDomainEventHandler : INotificationHandler<RoleCreatedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public RoleCreatedDomainEventHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(RoleCreatedNotification notification, CancellationToken cancellationToken)
    {
    }
}