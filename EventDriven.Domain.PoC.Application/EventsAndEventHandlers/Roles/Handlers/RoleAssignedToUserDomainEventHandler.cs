using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Handlers
{
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
}