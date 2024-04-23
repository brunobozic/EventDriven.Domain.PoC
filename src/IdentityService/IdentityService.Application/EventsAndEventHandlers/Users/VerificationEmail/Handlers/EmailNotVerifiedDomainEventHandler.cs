using IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;
using MediatR;
using Serilog;
using SharedKernel.DomainContracts;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Handlers;

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