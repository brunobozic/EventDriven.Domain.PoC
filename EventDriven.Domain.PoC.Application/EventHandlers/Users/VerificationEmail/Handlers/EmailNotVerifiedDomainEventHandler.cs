using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventHandlers.Users.VerificationEmail.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using Serilog;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.VerificationEmail.Handlers
{
    public class EmailNotVerifiedDomainEventHandler : INotificationHandler<EmailNotVerifiedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public EmailNotVerifiedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(EmailNotVerifiedNotification notification, CancellationToken cancellationToken)
        {
            Log.Information("Email verified domain event fired.");
        }
    }
}