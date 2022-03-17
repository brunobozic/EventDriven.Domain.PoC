using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Handlers
{
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
}