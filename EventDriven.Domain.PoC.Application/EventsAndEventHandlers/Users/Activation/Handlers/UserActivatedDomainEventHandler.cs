using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Notifications;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using Serilog;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Handlers
{
    public class UserActivatedDomainEventHandler : INotificationHandler<UserActivatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public UserActivatedDomainEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(UserActivatedNotification notification, CancellationToken cancellationToken)
        {
            Log.Information("User activated domain event fired.");
        }
    }
}