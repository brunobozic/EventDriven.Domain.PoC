using EventDriven.Domain.PoC.Application.DomainServices.JournalServices;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Handlers
{
    public class UserDeactivatedDomainEventHandler : INotificationHandler<UserDeactivatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserDeactivatedDomainEventHandler(ICommandsScheduler commandsScheduler, IJournalService journalService)
        {
            _commandsScheduler = commandsScheduler;
            JournalService = journalService;
        }

        private  IJournalService JournalService { get; }

        public async Task Handle(UserDeactivatedNotification notification, CancellationToken cancellationToken)
        {
            var journalEntry = (DateTime.UtcNow + " => Deactivated by [ " + notification.IntegrationEvent.DeactivatedBy.UserName + " ] : " + notification.IntegrationEvent.DeactivationReason);

            // delegate the rest of the operation to the journaling service
            try
            {
                var journalEntryMade = await JournalService.CreateAsync(journalEntry, notification.IntegrationEvent.DeactivatedBy.Id, notification.IntegrationEvent.UserId);
            }
            catch (Exception ex)
            {

            }
        }
    }
}