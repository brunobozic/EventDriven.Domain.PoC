using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.VerificationEmail.Handlers
{
    public class EmailVerifiedDomainEventHandler : INotificationHandler<EmailVerifiedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public EmailVerifiedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(EmailVerifiedNotification notification, CancellationToken cancellationToken)
        {
            // set something to something
        }
    }
}