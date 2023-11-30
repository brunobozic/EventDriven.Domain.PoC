using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.DomainEventDispatchers
{
    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
    {
        private readonly INotificationHandler<T> _decorated;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<T> decorated)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            var activitySource = new ActivitySource("OtPrGrJa");
            using var activity = activitySource.StartActivity("DomainEventsDispatcherNotificationHandlerDecorator");

            try
            {
                activity?.SetTag("Notification.Type", notification.GetType().Name);

                await _decorated.Handle(notification, cancellationToken);
                await _domainEventsDispatcher.DispatchEventsAsync();
            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
                activity?.SetStatus(ActivityStatusCode.Error);
                throw;
            }
        }

    }
}