using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.DomainServices.JournalServices;
using IdentityService.Application.EventsAndEventHandlers.Addresses.Notifications;
using MediatR;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.EventsAndEventHandlers.Addresses.Handlers;

public class AddressRemovedFromUserDomainEventHandler : INotificationHandler<AddressRemovedFromUserNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;
    private readonly IJournalService _journalService;

    public AddressRemovedFromUserDomainEventHandler(ICommandsScheduler commandsScheduler,
        IJournalService journalService)
    {
        _commandsScheduler = commandsScheduler;
        _journalService = journalService;
    }

    public async Task Handle(AddressRemovedFromUserNotification notification, CancellationToken cancellationToken)
    {
        var journalEntry = DateTime.UtcNow + " => [" + notification.IntegrationEvent.AddressTypeName +
                           "] address removed from user. Address removed: [" +
                           notification.IntegrationEvent.AddressLine1 + "].";

        // delegate the rest of the operation to the journaling service
        try
        {
            var journalEntryMade = await _journalService.CreateAsync(journalEntry,
                notification.IntegrationEvent.AddressRemoverId, notification.IntegrationEvent.UserId);
        }
        catch (Exception ex)
        {
        }
    }
}