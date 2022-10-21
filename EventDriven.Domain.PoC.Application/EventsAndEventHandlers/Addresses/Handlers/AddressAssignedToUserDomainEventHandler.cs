using EventDriven.Domain.PoC.Application.DomainServices.JournalServices;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Handlers
{
    public class AddressAssignedToUserDomainEventHandler : INotificationHandler<AddressAssignedToUserNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly IJournalService _journalService;

        public AddressAssignedToUserDomainEventHandler(ICommandsScheduler commandsScheduler, IJournalService journalService)
        {
            _commandsScheduler = commandsScheduler;
            _journalService = journalService;
        }

        async Task INotificationHandler<AddressAssignedToUserNotification>.Handle(AddressAssignedToUserNotification notification, CancellationToken cancellationToken)
        {
            // the journal message
            var journalEntry = DateTime.UtcNow + " => [" + notification.IntegrationEvent.AddressTypeName +
                                                  "] address assigned to user. Address assigned: [" +
                                                  notification.IntegrationEvent.AddressLine1 + "].";

            // delegate the rest of the operation to the journaling service
            try
            {
                var journalEntryMade = await _journalService.CreateAsync(journalEntry, notification.IntegrationEvent.AddressAssignerId, notification.IntegrationEvent.UserId);
            }
            catch (Exception ex)
            {
            }
        }
    }
}