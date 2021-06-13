using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventHandlers.Users.Activation.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.Activation.Handlers
{
    public class UserActivatedDomainEventHandler : INotificationHandler<UserActivatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserActivatedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserActivatedNotification notification, CancellationToken cancellationToken)
        {
        }
    }
}