using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventHandlers.Roles.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Roles.CUD.Handlers
{
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
}