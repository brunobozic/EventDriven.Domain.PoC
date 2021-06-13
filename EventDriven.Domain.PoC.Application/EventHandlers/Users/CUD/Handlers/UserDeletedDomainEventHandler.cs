using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Handlers
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