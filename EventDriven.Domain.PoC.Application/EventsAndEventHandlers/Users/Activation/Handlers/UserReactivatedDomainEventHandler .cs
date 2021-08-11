using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Handlers
{
    public class UserReactivatedDomainEventHandler : INotificationHandler<UserReactivatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserReactivatedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserReactivatedNotification notification, CancellationToken cancellationToken)
        {
            // Send welcome e-mail message...

            await _commandsScheduler.EnqueueAsync(new MarkUserAsWelcomedCommand(
                Guid.NewGuid(),
                notification.UserId));
        }
    }
}