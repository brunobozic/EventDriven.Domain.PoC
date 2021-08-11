using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Handlers
{
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
}