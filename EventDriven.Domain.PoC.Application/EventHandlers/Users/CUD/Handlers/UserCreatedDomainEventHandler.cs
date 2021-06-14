using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email.ActivationMail;
using EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Handlers
{
    public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserCreatedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            // verification mail needs to be sent whenever a user is created, we normally presume that at this point the user is not (soft)deleted nor deactivated

            // normally, this notification [UserCreatedNotification] would be handled by a separate micro-service after having been published / read from a queue (in the form of an integration event)
            // an email sending micro-service would then act on this integration event (consume it) and would finally send the actual email
            // in our case however, we are reacting to this notification **internally** whereby sending the email remains a "user CRM micro-service" (this micro-service) domain concern
            await _commandsScheduler.EnqueueAsync(new SendAccountVerificationMailCommand(
                Guid.NewGuid(),
                notification.ActivationLink,
                notification.ActivationLinkGenerated,
                notification.UserId,
                notification.Email,
                notification.FirstName,
                notification.LastName
            ));

            // failure to send the email needs to be regulated (this cant be done by using gmail, but by using mandril and similar e-mail sending services)
            // activation link will expire, user needs to click on the link provided in the email during the predefined time opening
            // user needs to click on the provided url (followed by entering a desired password) - after this the user has been fully registered
        }
    }
}