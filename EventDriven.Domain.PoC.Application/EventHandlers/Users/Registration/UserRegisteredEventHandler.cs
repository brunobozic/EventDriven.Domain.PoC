using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.Registration
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserRegisteredEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            // Send welcome e-mail message...

            await _commandsScheduler.EnqueueAsync(new MarkUserAsWelcomedCommand(
                Guid.NewGuid(),
                notification.UserId));
        }
    }
}