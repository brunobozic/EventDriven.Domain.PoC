using IdentityService.Application.DomainServices.JournalServices;
using IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using MediatR;
using SharedKernel.DomainContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Handlers;

public class UserDeactivatedDomainEventHandler : INotificationHandler<UserDeactivatedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public UserDeactivatedDomainEventHandler(ICommandsScheduler commandsScheduler, IJournalService journalService)
    {
        _commandsScheduler = commandsScheduler;
        JournalService = journalService;
    }

    private IJournalService JournalService { get; }

    public async Task Handle(UserDeactivatedNotification notification, CancellationToken cancellationToken)
    {
        var journalEntry = DateTime.UtcNow + " => Deactivated by [ " +
                           notification.DomainEvent.DeactivatedBy.UserName + " ] : " +
                           notification.DomainEvent.DeactivationReason;

        // delegate the rest of the operation to the journaling service
        try
        {
            var journalEntryMade = await JournalService.CreateAsync(journalEntry,
                notification.DomainEvent.DeactivatedBy.Id, notification.DomainEvent.UserId);
        }
        catch (Exception ex)
        {
        }
    }
}