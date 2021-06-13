using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using EventDriven.Domain.PoC.Domain.DomainEntities;
using EventDriven.Domain.PoC.Domain.DomainEntities.OutboxPattern;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Repository.EF.DomainEventDispatching
{
    public class IntegrationEventDispatcher : IDomainEventsDispatcher
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _scope;

        public IntegrationEventDispatcher(IMediator mediator, ILifetimeScope scope, DbContext context)
        {
            _mediator = mediator;
            _scope = scope;
            _context = context as ApplicationDbContext;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = _context.ChangeTracker
                .Entries<BasicDomainEntity<long>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var integrationEvents = new List<IIntegrationEvent<IDomainEvent>>();

            foreach (var intEvent in domainEvents)
            {
                var integrationEventType = typeof(IIntegrationEvent<>);
                var integrationEventWithGenericType =
                    integrationEventType.MakeGenericType(intEvent.GetType());
                var integrationEvent = _scope.ResolveOptional(integrationEventWithGenericType, new List<Parameter>
                {
                    new NamedParameter("integrationEvent", intEvent)
                });

                if (integrationEvent != null)
                    integrationEvents.Add(integrationEvent as IIntegrationEvent<IDomainEvent>);
            }

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async domainEvent => { await _mediator.Publish(domainEvent); });

            await Task.WhenAll(tasks);

            // 2PC problem is solved by using an outbox table as a queue 
            // each and every event that needs to be published so other microservices can react to it
            // is first inserted into the local Db (the outbox table) as a single transaction
            // then, a background job (Quartz) will read rows from the outbox table and attempt publishing then unto a message queue (or a kafka topic)
            // this is the only way in which we can guarantee delivery (at least once delivery guarantee) without resorting to distributed transactions which are, in most cases, a no go
            foreach (var integrationEvent in integrationEvents)
            {
                var type = integrationEvent.GetType().FullName;
                var data = JsonConvert.SerializeObject(integrationEvent);
                var outboxMessage = new OutboxMessage(
                    integrationEvent.IntegrationEvent.OccurredOn,
                    type,
                    data);
                await _context.OutboxMessages.AddAsync(outboxMessage);
            }
        }
    }
}