using EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Handlers
{
    public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserCreatedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            await _commandsScheduler.EnqueueAsync(new SendAccountVerificationMailCommand(
                      notification.ActivationLink,
                      notification.ActivationLinkGenerated,
                      notification.UserId,
                      notification.Email,
                      notification.UserName,
                      notification.FirstName,
                      notification.LastName,
                      notification.Origin
                  ));

        }
    }
}