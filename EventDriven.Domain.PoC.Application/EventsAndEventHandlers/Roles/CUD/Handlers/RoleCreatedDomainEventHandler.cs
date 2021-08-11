using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.CUD.Handlers
{
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
}