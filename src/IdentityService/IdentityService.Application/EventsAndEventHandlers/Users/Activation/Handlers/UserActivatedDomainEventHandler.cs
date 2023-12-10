using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.DomainServices.JournalServices;
using IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Handlers;

public class UserActivatedDomainEventHandler : INotificationHandler<UserActivatedNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;
    private readonly IJournalService journalService;

    public UserActivatedDomainEventHandler(ICommandsScheduler commandsScheduler, IJournalService journalService)
    {
        _commandsScheduler = commandsScheduler;
        this.journalService = journalService;
    }

    public async Task Handle(UserActivatedNotification notification, CancellationToken cancellationToken)
    {
        // the journal message
        var journalEntry = DateTime.UtcNow + " => Activated by [ " + notification.IntegrationEvent.ActivatedByUsername +
                           " ] .";

        // delegate the rest of the operation to the journaling service
        try
        {
            var journalEntryMade = await journalService.CreateAsync(journalEntry,
                notification.IntegrationEvent.ActivatedById, notification.IntegrationEvent.UserId);
        }
        catch (Exception ex)
        {
        }
    }
}