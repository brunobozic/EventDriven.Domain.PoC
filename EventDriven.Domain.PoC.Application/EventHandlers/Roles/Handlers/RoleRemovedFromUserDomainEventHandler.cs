using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventHandlers.Roles.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Roles.Handlers
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