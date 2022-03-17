using EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Registration
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
            await _commandsScheduler.EnqueueAsync(new MarkUserAsWelcomedCommand(
                Guid.NewGuid(),
                notification.UserId));
        }
    }
}