using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.CommandsAndHandlers.Users.Email.ActivationMail;
using IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Handlers;

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