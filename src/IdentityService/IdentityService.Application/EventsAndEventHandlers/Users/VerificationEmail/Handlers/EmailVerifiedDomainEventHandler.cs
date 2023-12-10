using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;
using MediatR;
using Serilog;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Handlers;

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
        Log.Debug("EmailVerifiedDomainEventHandler");
    }
}