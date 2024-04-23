using IdentityService.Application.DomainServices.JournalServices;
using IdentityService.Application.EventsAndEventHandlers.Addresses.Notifications;
using MediatR;
using SharedKernel.DomainContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Application.EventsAndEventHandlers.Addresses.Handlers;

public class AddressAssignedToUserDomainEventHandler : INotificationHandler<AddressAssignedToUserNotification>
{
    private readonly ICommandsScheduler _commandsScheduler;
    private readonly IJournalService _journalService;

    public AddressAssignedToUserDomainEventHandler(ICommandsScheduler commandsScheduler, IJournalService journalService)
    {
        _commandsScheduler = commandsScheduler;
        _journalService = journalService;
    }

    async Task INotificationHandler<AddressAssignedToUserNotification>.Handle(
        AddressAssignedToUserNotification notification, CancellationToken cancellationToken)
    {
        // the journal message
        var journalEntry = DateTime.UtcNow + " => [" + notification.DomainEvent.AddressTypeName +
                           "] address assigned to user. Address assigned: [" +
                           notification.DomainEvent.AddressLine1 + "].";

        // delegate the rest of the operation to the journaling service
        try
        {
            var journalEntryMade = await _journalService.CreateAsync(journalEntry,
                notification.DomainEvent.AddressAssignerId, notification.DomainEvent.UserId);
        }
        catch (Exception ex)
        {
        }
    }
}